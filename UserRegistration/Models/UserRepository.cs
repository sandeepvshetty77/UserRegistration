using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
            user.Password = EncryptPassword(user.Password);
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public string EncryptPassword(string password)
        {
            var data = Encoding.UTF8.GetBytes(password);

            var encrypted_data = ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);

            return Convert.ToBase64String(encrypted_data);
        }

        public User GetUserByUserName(string userName)
        {
            return _context.Users.FirstOrDefault(user => user.Username.ToLower().CompareTo(userName.ToLower()) == 0);
        }
    }
}
