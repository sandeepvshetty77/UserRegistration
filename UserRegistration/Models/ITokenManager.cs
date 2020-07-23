using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserRegistration.Models
{
    public interface ITokenManager
    {
        string GenerateToken(UserViewModel userViewModel);
        void SaveToken(string token);
    }
}
