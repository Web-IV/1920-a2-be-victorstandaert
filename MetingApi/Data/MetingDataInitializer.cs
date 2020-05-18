using Microsoft.AspNetCore.Identity;
using MetingApi.Models;
using System.Linq;
using System.Threading.Tasks;
using Project.Models;
using System;

namespace MetingApi.Data
{
    public class MetingDataInitializer
    {
        private readonly MetingContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public MetingDataInitializer(MetingContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task InitializeData()
        {
            _dbContext.Database.EnsureDeleted();
            if (_dbContext.Database.EnsureCreated())
            {
                //seeding the database with metingen, see DBContext       
                User user1 = new User { Email = "recipemaster@hogent.be", FirstName = "Adam", LastName = "Master" };
                user1.AddMeting(new Meting { Created = DateTime.Now, User = user1, MetingResultaat = 25}); //meting van account
                _dbContext.users.Add(user1);
                await CreateUser(user1.Email, "P@ssword1111");

                User user2 = new User { Email = "student@hogent.be", FirstName = "Student", LastName = "Hogent" };
                user2.AddMeting(new Meting { Created = DateTime.Now, User = user2, MetingResultaat = -5 }); //meting van account
                _dbContext.users.Add(user2);
                await CreateUser(user2.Email, "P@ssword1111");


                _dbContext.SaveChanges();
            }
        }

        private async Task CreateUser(string email, string password)
        {
            var user = new IdentityUser { UserName = email, Email = email };
            await _userManager.CreateAsync(user, password);
        }
    }
}

