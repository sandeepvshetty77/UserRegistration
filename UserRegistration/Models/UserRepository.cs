using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserRegistration.Models
{
    public class UserRepository : IUserRepository
    {
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        private AppDbContext _context { get; }

        public User CreateUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public User GetUserByUserName(string userName)
        {
            return _context.Users.FirstOrDefault(user => user.Username.ToLower().CompareTo(userName.ToLower()) == 0);
        }
    }
}
