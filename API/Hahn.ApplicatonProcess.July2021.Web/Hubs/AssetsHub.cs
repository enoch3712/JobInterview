using Hahn.ApplicationProcess.July2021.Data.DTOs;
using Hahn.ApplicationProcess.July2021.Data.Service.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hahn.ApplicationProcess.July2021.Web.Hubs
{
    public class AssetsHub : Hub
    {
        private readonly IGetAssetsRequestService _getAssetsRequestService;

        public AssetsHub(IGetAssetsRequestService getAssetsRequestService)
        {
            _getAssetsRequestService = getAssetsRequestService;
        }

        public async Task<IReadOnlyList<AssetDto>> Get()
        {
            var result = await _getAssetsRequestService.ExecuteAsync();

            return result;
        }
    }
}
