
namespace UserRegistration.Models
{
    public interface IUserRepository
    {
        User CreateUser(User user);
        User GetUserByUserName(string userName);
    }
}
