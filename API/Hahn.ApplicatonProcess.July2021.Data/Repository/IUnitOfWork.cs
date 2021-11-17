using Microsoft.EntityFrameworkCore;

namespace Hahn.ApplicationProcess.July2021.Data.Repository
{
    public interface IUnitOfWork
    {
        DbContext Context { get; }
        void Commit();
        void Rewind();
    }
}
