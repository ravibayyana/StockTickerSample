using System.Reactive.Concurrency;

namespace StockTickerServer.Utils
{
    public interface ICustomScheduler
    {
        IScheduler TaskPool { get; }
    }

    public class CustomScheduler : ICustomScheduler
    {
        public IScheduler TaskPool => TaskPoolScheduler.Default;
    }
}
