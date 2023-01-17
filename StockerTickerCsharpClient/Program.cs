using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using StockTickerServer.Models;

namespace StockerTickerCsharpClient
{
    internal class Program
    {
        private static HubConnection? _connection;
        public static async Task Main(string[] args)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:60101/stockTickerHub",
                    options => { options.Transports = HttpTransportType.WebSockets; })
                .WithAutomaticReconnect()
                .Build();

            OnClosedEvent();

            try
            {
                await _connection.StartAsync();

                //Receive message from server                
                _connection.On<Stock[]>("UpdateStockPrices",
                    messages =>
                    {
                        messages.ToList().ForEach(x => Console.WriteLine("Received Stock: " + x));
                        Console.WriteLine("=======================================");
                    });

                Console.WriteLine("Connection started");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connection Error: " + ex.Message);
            }

            Console.ReadLine();
        }

        public static void OnClosedEvent()
        {
            _connection.Closed += async error =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await _connection.StartAsync();
            };
        }

    }

}