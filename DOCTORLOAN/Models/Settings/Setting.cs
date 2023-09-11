namespace DOCTORLOAN.Models.Settings;

public class Setting
{
    public string Name { get; set; }
    public string Value { get; set; }
    public int SystemId { get; set; }
    public SystemSettingType SettingApp
    {
        get => (SystemSettingType)SystemId;
        set => SystemId = (int)value;
    }
    public override string ToString()
    {
        return Name;
    }
}

public enum SystemSettingType
{
    All = 0,
    MobileApp = 1,
    Webadmin = 2,
}