using Hahn.ApplicationProcess.July2021.Domain;

namespace Hahn.ApplicationProcess.July2021.Data.Repository
{
    public interface IUserRepository
    {
        User[] GetUsers();
        User GetById(int id);

        User Add(User user);
        void Update(User user);
        void Delete(User user);
    }
}
