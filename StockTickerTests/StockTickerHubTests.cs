using Microsoft.AspNetCore.SignalR;
using Microsoft.Reactive.Testing;
using Moq;
using StockTickerServer.Hubs;
using StockTickerServer.Models;
using StockTickerServer.Services;
using StockTickerServer.Utils;

namespace StockTickerTests
{
    public class StockTickerHubTests
    {
        private readonly TestScheduler _testScheduler;

        public StockTickerHubTests()
        {
            _testScheduler = new TestScheduler();
        }

        [Fact]
        public void StockTickerHubTest()
        {
            var clientMoq = new Mock<IStockTickerHubMessageToClient>();
            clientMoq.Setup(x =>x.UpdateStockPrices(It.IsAny<List<Stock>>())).Verifiable();
            var mockClients = new Mock<IHubClients<IStockTickerHubMessageToClient>>();

            mockClients.Setup(x => x.All).Returns(clientMoq.Object);
            var hubContextMoq = new Mock<IHubContext<StockTickerHub, IStockTickerHubMessageToClient>>();

            hubContextMoq.Setup(x => x.Clients).Returns(mockClients.Object);

            var customSchedulerMoq= new Mock<ICustomScheduler>();
            customSchedulerMoq.Setup(x => x.TaskPool).Returns(_testScheduler);

            var stockTickerService = new StockTickerService(hubContextMoq.Object, customSchedulerMoq.Object);

            var hub = new StockTickerHub(stockTickerService);
            _testScheduler.AdvanceBy(TimeSpan.FromSeconds(1).Ticks);

            clientMoq.Verify(x => x.UpdateStockPrices(It.IsAny<List<Stock>>()), Times.Once);

            _testScheduler.AdvanceBy(TimeSpan.FromSeconds(1).Ticks);
            clientMoq.Verify(x => x.UpdateStockPrices(It.IsAny<List<Stock>>()), Times.Exactly(2));
        }
    }
}