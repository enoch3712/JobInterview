using Hahn.ApplicationProcess.July2021.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Hahn.ApplicationProcess.July2021.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly Context _context;

        public UserRepository(Context context)
        {
            _context = context;
        }

        public User[] GetUsers() => 
            _context.Users
                .Include(u => u.Assets)
                .ToArray();

        public User GetById(int id) => 
            _context.Users
                .Include(u => u.Assets)
                .FirstOrDefault(u => u.Id == id);

        public User Add(User user)
        {
            _context.Users.Add(user);

            return user;
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
        }

        public void Delete(User user)
        {
            _context.Users.Remove(user);
        }
    }
}