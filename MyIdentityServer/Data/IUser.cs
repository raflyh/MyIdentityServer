using MyIdentityServer.ViewModels;

namespace MyIdentityServer.Data
{
    public interface IUser
    {
        Task Registration(UserCreateViewModel createUser);
        IEnumerable<UserViewModel> GetAllUsers();
        Task<UserViewModel> Authenticate(string username, string password);
    }
}
