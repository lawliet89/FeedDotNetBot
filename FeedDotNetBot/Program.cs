using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FeedDotNetBot.Properties;
using Nito.AsyncEx;

namespace FeedDotNetBot
{
    class Program
    {
        private static TaskCompletionSource<int> mainTaskCompletionSource;

        // Entry point to the bot
        static int Main(string[] args)
        {
            return AsyncContext.Run(AsyncMain);
        }

        static async Task<int> AsyncMain()
        {
            mainTaskCompletionSource = new TaskCompletionSource<int>();
            Console.CancelKeyPress += (s, e) => Terminate();

            if (string.IsNullOrEmpty(Settings.Default.TelegramApiKey))
            {
                Console.WriteLine(
                    "You need to open 'FeedDotNetBot.exe.config.xml' and put your Telegram API key in there");
                Terminate(1, false);
                return await mainTaskCompletionSource.Task;
            }
            else
            {
                Console.WriteLine("API Key: {0}", Settings.Default.TelegramApiKey);
                Console.WriteLine("Press CTRL + C to exit");
                return await run();
            }
        }

        private static async Task<int> run()
        {
            var runnables = new IDisposableRunnable[] {new Bot()};
            try
            {
                var aggregateTasks = new List<Task>();
                aggregateTasks.AddRange(runnables.Select(runnable => runnable.Run()));
                aggregateTasks.Add(mainTaskCompletionSource.Task);
                await Task.WhenAny(aggregateTasks);
                return mainTaskCompletionSource.Task.IsCompleted ? mainTaskCompletionSource.Task.Result : 1;
            }
            finally
            {
                foreach (var runnable in runnables)
                    runnable.Dispose();
            }
        }

        public static void Terminate(int code = 0, bool terminateImmediately = true)
        {
            if (!terminateImmediately)
            {
                Console.WriteLine("Press enter to exit");
                Console.Read();
            }
            mainTaskCompletionSource.SetResult(code);
        }
    }
}
