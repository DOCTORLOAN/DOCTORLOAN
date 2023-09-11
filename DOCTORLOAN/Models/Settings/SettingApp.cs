namespace DOCTORLOAN.Models.Settings;

public class SettingApp
{
    //public AppSetupType Type { get; set; }
    public string AppVersion { get; set; }
    public string PrefixCode { get; set; }
    public string PrefixChildCode { get; set; }
    public string KeyValue { get; set; }
    public bool IsActive { get; set; }

    public virtual ICollection<SettingAppContent> SettingAppContents { get; } = new HashSet<SettingAppContent>();
}