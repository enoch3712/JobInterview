using System.Collections.Generic;

namespace Hahn.ApplicationProcess.July2021.Data.DTOs
{
    // in the ideal world this should be struct record, to avoid heap allocation
    public record CoinCapAssetsDto
    {
        public List<AssetDto> data { get; set; }
        public long timestamp { get; set; }
    }
}
