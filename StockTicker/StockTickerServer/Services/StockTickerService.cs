using System.Reactive.Linq;
using Microsoft.AspNetCore.SignalR;
using StockTickerServer.Hubs;
using StockTickerServer.Models;
using StockTickerServer.Utils;

namespace StockTickerServer.Services
{
    public interface IStockTickerService
    {
         void GetStockPrices();
    }

    public class StockTickerService : IStockTickerService, IDisposable
    {
        private readonly IHubContext<StockTickerHub, IStockTickerHubMessageToClient> _stockTickerHub;
        private readonly ICustomScheduler _customScheduler;
        private readonly Dictionary<string, Stock> _defaultStocks;
        private readonly Random _random;
        private readonly  IDisposable _priceTickingSubscription;

        public StockTickerService(
            IHubContext<StockTickerHub, 
            IStockTickerHubMessageToClient> stockTickerHub, 
            ICustomScheduler customScheduler)
        {
            _stockTickerHub = stockTickerHub;
            _customScheduler = customScheduler;
            _defaultStocks = new Dictionary<string, Stock>();
            _random = new Random(100);

            this.InitDefaultStocks();
            _priceTickingSubscription = this.StartPriceTickingAtEverySec();
        }

        private  IDisposable StartPriceTickingAtEverySec()
        {
            return Observable.Interval(TimeSpan.FromSeconds(1), _customScheduler.TaskPool)
                .Subscribe(async x =>
                {
                    Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} ========================== START");
                    foreach (var stock in _defaultStocks)
                    {
                        stock.Value.Price = _random.NextDouble();
                        stock.Value.UpdatedAt = DateTime.UtcNow;
                        Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} {x} Updated Stock Price: {stock}");
                    }

                    Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} ========================== END");
                    await _stockTickerHub.Clients.All.UpdateStockPrices(_defaultStocks.Values.ToList());
                });
        }

        private void InitDefaultStocks()
        {
            var stocks = new List<string>()
            {
                "APPL",
                "GOOG",
                "TSLA",
                "CS",
                "MSFT",
                "PEP",
                "COKE",
                "JPM",
                "MS",
                "CITI"
            };

            stocks.ForEach(stock => 
                _defaultStocks.Add(
                    stock, 
                    Stock.CreateNew(stock, _random.NextDouble())
                    )
                );
        }

        public void GetStockPrices()
        {

        }

        public void Dispose()
        {
            _priceTickingSubscription.Dispose();
        }
    }
}
