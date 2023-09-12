using DOCTORLOAN.Models.Users;

namespace DOCTORLOAN.service
{
    public class LoginService : ILoginService
    {
        public async Task<User> Login(string userName, string password)
        {
            //User user = await Queryable.Where(e => e.Username == userName || e.P == userName).FirstOrDefaultAsync();
            //if (user != null)
            //{
            //    //if (user.Active == false)
            //    //{
            //    //    throw new Exception("Tài khoản chưa được kích hoạt");
            //    //}
            //    //if (user.Password == SecurityUtilities.HashSHA1(password))
            //    //{
            //    //    return user;
            //    //}
            //}
            return null;
        }
    }
}
