namespace DOCTORLOAN.Models.Settings;

public class SettingUserLog
{
    public int UserId { get; set; }
    public int SettingUserId { get; set; }
    //public SettingUserType Type { get; set; }
    public bool IsAgree { get; set; }
}