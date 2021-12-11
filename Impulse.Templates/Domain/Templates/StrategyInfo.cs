namespace Impulse.Shared.Domain.Templates
{
    public class StrategyInfo
    {
        /// <summary>
        /// Strategy id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Trading symbol
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Storage path
        /// </summary>
        public string StoragePath { get; set; }

        /// <summary>
        /// Condition how much the price must drop(in percentage)
        /// </summary>
        public int BidRatio { get; set; }

        /// <summary>
        /// Condition how much the price must increase(in percentage)
        /// </summary>
        public int AskRatio { get; set; }
    }
}   