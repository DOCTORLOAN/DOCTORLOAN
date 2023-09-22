namespace DOCTORLOAN.Models.Users;

public class UserDevice
{
    public int UserId { get; set; }
    public int DeviceId { get; set; }
    public bool IsActive { get; set; }

    public virtual Device Device { get; set; }
    public virtual User User { get; set; }
}