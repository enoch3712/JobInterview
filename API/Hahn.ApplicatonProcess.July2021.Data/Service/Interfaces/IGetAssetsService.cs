using Hahn.ApplicationProcess.July2021.Domain;
using System.Threading.Tasks;

namespace Hahn.ApplicationProcess.July2021.Data.Service.Interfaces
{
    public interface IGetAssetsService
    {
        Task<Asset[]> ExecuteAsync();
    }
}
