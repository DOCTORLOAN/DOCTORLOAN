namespace DOCTORLOAN.Models.Users;

public class UserSource
{
    public string Description { get; set; }
    public string Name { get; set; }
    public int? ParentId { get; set; }
    public int? EventId { get; set; }
    public bool IsDelete { get; set; }
}
