namespace DOCTORLOAN.Models.Roles;

public class Role
{
    public int DepartmentId { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public int? Level { get; set; }
    public int OrderBy { get; set; }
    public bool IsActive { get; set; }

    //public virtual Department Department { get; set; }
    //public virtual ICollection<User> Users { get; set; } = new List<User>();
}
