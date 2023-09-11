namespace DOCTORLOAN.Models.Users;

public class UserAddress
{
    public int UserId { get; set; }
    public int AddressId { get; set; }

    public string FullName { get; set; }
    public string PhoneCode { get; set; }
    public string Phone { get; set; }
    //public UserAddressType Type { get; set; }
    public string Remarks { get; set; }
    public bool IsDefault { get; set; }
    public bool IsDelete { get; set; }

    public virtual User User { get; set; }
    //public virtual Address Address { get; set; }
}