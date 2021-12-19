namespace Impulse.Shared.Templates
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
        /// Condition how much the price must increase(in percentage)
        /// </summary>
        public int Rise { get; set; }

        /// <summary>
        /// Condition how much the price must drop(in percentage)
        /// </summary>
        public int Drop { get; set; }

        /// <summary>
        /// Calculated avarage 
        /// </summary>
        public int Average { get; set; }
        
        /// <summary>
        /// Condition to count interval avg or curr value
        /// </summary>
        public int Ticker { get; set; }

        public int StopLosePercentageDown { get; set; }

        public int FundPercentage { get; set; }

        public int StopLoseType { get; set; }
    }
}   