namespace Hahn.ApplicationProcess.July2021.Domain
{
    public class Asset
    {
        public string Id { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public double PriceUsd { get; set; }
        public double ChangePercent24Hr { get; set; }
    }
}
