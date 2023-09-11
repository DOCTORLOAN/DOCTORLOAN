namespace DOCTORLOAN.Models.Settings;

public class SettingAppContent
{
    public int SettingAppId { get; set; }
    //public LanguageEnum Language { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }

    public virtual SettingApp SettingApp { get; set; }
}
