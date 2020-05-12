using MetingApi.Data;
using Microsoft.EntityFrameworkCore;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MetingContext _context;
        private readonly DbSet<User> _users;

        public UserRepository(MetingContext dbContext)
        {
            _context = dbContext;
            _users = dbContext.users;
        }

        public User GetBy(string email)
        {
            return _users.Include(c => c.Metingen).ThenInclude(u => u.Resultaten).SingleOrDefault(c => c.Email == email);
        }

        public void Add(User user)
        {
            _users.Add(user);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}

