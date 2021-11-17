using Hahn.ApplicationProcess.July2021.Data.DTOs;
using Hahn.ApplicationProcess.July2021.Data.Service.Interfaces;
using Hahn.ApplicationProcess.July2021.Domain;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Hahn.ApplicationProcess.July2021.Data.Service.Implementations
{
    //the result should be cached
    public class GetAssetsService : IGetAssetsService
    {
        private const string CacheEntry = "assets";

        private readonly IMemoryCache _memoryCache;
        private readonly IGetAssetsRequestService _getAssetsRequestService;

        public GetAssetsService(IMemoryCache memoryCache, IGetAssetsRequestService getAssetsRequestService)
        {
            _memoryCache = memoryCache;
            _getAssetsRequestService = getAssetsRequestService;
        }

        public async Task<Asset[]> ExecuteAsync()
        {
            if (_memoryCache.TryGetValue(CacheEntry, out Asset[] data))
            {
                return data;
            }

            var queryResult = await _getAssetsRequestService.ExecuteAsync();

            var result = queryResult.Select(MapAssets).ToArray();

            _memoryCache.Set(CacheEntry, result, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5)));

            return result;
        }

        // i don´t like autoMapper
        private static Asset MapAssets(AssetDto input) => 
            new()
            {
                Id = input.id, 
                Name = input.name, 
                Symbol = input.symbol, 
                PriceUsd = Math.Round(double.Parse(input.priceUsd), 2), 
                ChangePercent24Hr = Math.Round(double.Parse(input.changePercent24Hr), 2)
            };
    }
}
