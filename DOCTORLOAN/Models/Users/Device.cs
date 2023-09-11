namespace DOCTORLOAN.Models.Users;

public class Device
{

    //public PlatformType Platform { get; set; }

    public string DeviceId { get; set; }

    public string DeviceName { get; set; }

    public string Version { get; set; }

    public string VersionApp { get; set; }

    public string Object { get; set; }

    public string IsActive { get; set; }


    public virtual ICollection<UserDevice> UserDevices { get; set; } = new List<UserDevice>();
}