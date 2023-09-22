namespace DOCTORLOAN.Models.Addresses;

public class Province
{
    public string Name { get; set; }
    public string Code { get; set; }
    public string Type { get; set; }
    public int? PhoneCode { get; set; }
    public string ZipCode { get; set; }
    public int CountryId { get; set; }
    //public RegionType? RegionType { get; set; }
    public int? SortBy { get; set; }
    public bool IsPublished { get; set; }
    public bool IsDelete { get; set; }

    public virtual Country Country { get; set; }
    public virtual ICollection<District> Districts { get; set; } = new List<District>();
    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
}