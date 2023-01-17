namespace StockTickerServer.Models
{
    public class Stock
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Price{ get; set; }
        public DateTime UpdatedAt { get; set; }

        public static Stock CreateNew(string stockName, double price)
        {
            return new Stock()
            {
                Id = stockName,
                Name = stockName,
                Price = price,
                UpdatedAt = DateTime.UtcNow,
            };
        }

        public override string ToString()
        {
            return $"{Name} -- {Price} -- {UpdatedAt}";
        }
    }
}
