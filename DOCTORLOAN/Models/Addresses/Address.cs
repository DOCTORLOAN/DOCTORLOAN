namespace DOCTORLOAN.Models.Addresses;

public class Address
{
    public string AddressLine { get; set; }
    public int CountryId { get; set; }
    public int? ProvinceId { get; set; }
    public int? DistrictId { get; set; }
    public int? WardId { get; set; }

    public int? TotalRelated { get; set; }

    public virtual Country Country { get; set; }
    public virtual Province Province { get; set; }
    public virtual District District { get; set; }
    public virtual Ward Ward { get; set; }
}
