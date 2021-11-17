using Microsoft.EntityFrameworkCore;
using System;

namespace Hahn.ApplicationProcess.July2021.Data.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private bool _isAlive = true;
        public UnitOfWork(Context context)
        {
            Context = context;
        }

        public DbContext Context { get; }

        public void Commit()
        {
            if (!_isAlive)
                return;

            try
            {
                Context.SaveChanges();
            }
            finally
            {
                _isAlive = false;
            }
        }

        public void Rewind() => _isAlive = true;

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
