namespace DOCTORLOAN.Models.Users;

public class UserDetail
{
    public int UserId { get; set; }
    public int? JobId { get; set; }
    public string Facebook { get; set; }
    public string Tiktok { get; set; }
    public string Zalo { get; set; }
    public int CountryId { get; set; }
    public DateTimeOffset? LastedSignIn { get; set; }

    //public virtual Job Job { get; set; }
}