using System;
using System.Threading;
using System.Threading.Tasks;

namespace FeedDotNetBot
{
    interface IDisposableRunnable : IDisposable
    {
        Task Run();
    }
}
