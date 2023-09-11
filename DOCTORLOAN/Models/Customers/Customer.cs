namespace DOCTORLOAN.Models.Customers;

public class Customer
{
    public Guid UID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    //public Gender Gender { get; set; }
    public DateTimeOffset? DOB { get; set; }
    public string PasswordHash { get; set; }

    public bool IsDelete { get; set; }

    public virtual ICollection<CustomerAddress> CustomerAddresses { get; set; } = new List<CustomerAddress>();

}