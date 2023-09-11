namespace DOCTORLOAN.Models.Users;

public class UserActivity
{
    public int UserId { get; set; }
    //public UserActivityType ActivityType { get; set; }
    public bool IsDelete { get; set; }

    public virtual User User { get; set; }
}
