namespace DOCTORLOAN.Models.Settings;

public class SettingUser
{
    public int UserId { get; set; }
    //public SettingUserType Type { get; set; }
    public bool IsAgree { get; set; }
    public bool IsDelete { get; set; }
    //public virtual User Users { get; set; }
    public virtual ICollection<SettingUserLog> SettingUserLogs { get; set; } = new List<SettingUserLog>();
}