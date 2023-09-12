using DOCTORLOAN.Models.Users;

namespace DOCTORLOAN.service
{
    public interface ILoginService
    {
        Task<User> Login(string userName, string password);
    }
}
