using Hahn.ApplicationProcess.July2021.Domain;

namespace Hahn.ApplicationProcess.July2021.Data.Service.Interfaces
{
    public interface IUpdateService
    {
        bool Execute(int id, User user);
    }
}
