namespace DOCTORLOAN.Models.VMAuth
{
    public class Signin
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool KeepLoggedIn { get; set; }
    }
}
