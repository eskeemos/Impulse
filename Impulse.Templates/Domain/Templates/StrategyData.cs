namespace Impulse.Shared.Domain.Templates
{
    public class StrategyData
    {
        // Strategy id
        public int Id { get; set; }

        // Symbol for trading
        public string Symbol { get; set; }

        // Path with data files
        public string StoragePath { get; set; }

        // Highest price
        public int BidRatio { get; set; }

        // Lower price
        public int AskRatio { get; set; }
    }

}   