using Hahn.ApplicationProcess.July2021.Data.DTOs;
using Hahn.ApplicationProcess.July2021.Data.Service.Interfaces;
using Hahn.ApplicationProcess.July2021.Domain;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hahn.ApplicationProcess.July2021.Data.Service.Implementations
{
    public class GetAssetsRequestService : IGetAssetsRequestService
    {
        private const string Uri = "assets";

        public async Task <IReadOnlyList<AssetDto>> ExecuteAsync()
        {
            var client = new RestClient(EnvironmentVariables.CoinCapUrl);
            var request = new RestRequest(Uri, Method.GET);

            var queryResult = await client.ExecuteAsync<CoinCapAssetsDto>(request);

            return queryResult.Data.data;
        }
    }
}
