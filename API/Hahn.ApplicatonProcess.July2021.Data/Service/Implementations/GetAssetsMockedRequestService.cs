using Hahn.ApplicationProcess.July2021.Data.DTOs;
using Hahn.ApplicationProcess.July2021.Data.Service.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hahn.ApplicationProcess.July2021.Data.Service.Implementations
{
    public class GetAssetsMockedRequestService : IGetAssetsRequestService
    {
        public async Task<IReadOnlyList<AssetDto>> ExecuteAsync()
        {
            var path = Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Files/AssetMock.json");

            var result = JsonSerializer.Deserialize<CoinCapAssetsDto>(await File.ReadAllTextAsync(path));

            return result.data.ToArray();
        }
    }
}