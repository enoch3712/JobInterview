using Hahn.ApplicationProcess.July2021.Data.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hahn.ApplicationProcess.July2021.Data.Service.Interfaces
{
    public interface IGetAssetsRequestService
    {
        Task<IReadOnlyList<AssetDto>> ExecuteAsync();
    }
}
