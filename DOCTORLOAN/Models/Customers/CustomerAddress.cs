namespace DOCTORLOAN.Models.Customers;

public class CustomerAddress
{
    public int CustomerId { get; set; }
    public int AddressId { get; set; }
    //public AddressType Type { get; set; }

    public string FullName { get; set; }
    public string Phone { get; set; }
    public string Remarks { get; set; }
    public bool IsDefault { get; set; }
    public bool IsDelete { get; set; }

    public virtual Customer Customer { get; set; }
    //public virtual Address Address { get; set; }
}
