using System;
using System.Threading;
using System.Threading.Tasks;
using FeedDotNetBot.Properties;
using Nito.AsyncEx;
using Telegram.Bot;

namespace FeedDotNetBot
{
    public class Bot : IDisposableRunnable
    {
        private readonly AsyncContextThread asyncContextThread;
        private readonly CancellationTokenSource cancellationTokenSource;
        private readonly Api api;
        private int updateOffset = 0;

        public Bot()
        {
            api = new Api(Settings.Default.TelegramApiKey);
            cancellationTokenSource = new CancellationTokenSource();
            asyncContextThread = new AsyncContextThread();
        }

        public Task Run()
        {
            return asyncContextThread.Factory.Run(async () =>
            {
                // First let's check that we actually have the right tokens
                try
                {
                    var user = await GetNameAsync();
                    Console.WriteLine("I am {0}!", user);
                    await scheduleUpdatesAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Program.Terminate(1, false);
                }
            });
        }

        public async Task<string> GetNameAsync()
        {
            var user = await api.GetMe();
            return user.Username;
        }

        private async Task scheduleUpdatesAsync()
        {
            var interval = Settings.Default.BotUpdateInterval;
            while (true)
            {
                if (!cancellationTokenSource.IsCancellationRequested)
                {
                    await getUpdatesAsync();
                    await Task.Delay(interval);
                }
                else
                {
                    break;
                }
            }
        }

        private async Task getUpdatesAsync()
        {
            var lastUpdateOffset = updateOffset;
            var updates = await api.GetUpdates(lastUpdateOffset);
            
            foreach (var update in updates)
            {
                lastUpdateOffset = update.Id;
                Console.WriteLine(update.Message.Text);
            }
            updateOffset = lastUpdateOffset + 1;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
            asyncContextThread.Dispose();
        }
    }
}
