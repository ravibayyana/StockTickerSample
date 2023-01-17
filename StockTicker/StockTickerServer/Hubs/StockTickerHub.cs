using Microsoft.AspNetCore.SignalR;
using StockTickerServer.Models;
using StockTickerServer.Services;

namespace StockTickerServer.Hubs
{
    public interface IStockTickerHubMessageToClient
    {
        Task UpdateStockPrices(List<Stock> stocks);
    }

    public class StockTickerHub : Hub<IStockTickerHubMessageToClient>
    {
        private readonly IStockTickerService _stockTickerService;

        public StockTickerHub(IStockTickerService stockTickerService)
        {
            _stockTickerService = stockTickerService;
        }

        //public void GetStockPrices()
        //{
        //    _stockTickerService.GetStockPrices();
        //}

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"********** Client DISCONNECTED with Id: {Context.ConnectionId}");
            Console.ResetColor();

            return base.OnDisconnectedAsync(exception);
        }


        public override Task OnConnectedAsync()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"*********** Client CONNECTED with Id: {Context.ConnectionId}");
            Console.ResetColor();

            return base.OnConnectedAsync();
        }
    }
}
