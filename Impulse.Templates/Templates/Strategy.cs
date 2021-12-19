namespace Impulse.Shared.Templates
{
    // TOCOM
    public class Strategy
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string StoragePath { get; set; }
        public int PercentagePriceRise { get; set; }
        public int PercentagePriceDrop { get; set; }
        public int AveragesAmount { get; set; }
        public int NowAvgPrice { get; set; }
        public int PercentageStopLose { get; set; }
        public int PercentageResourceToPlay { get; set; }
        public int SellType { get; set; }
    }
}   