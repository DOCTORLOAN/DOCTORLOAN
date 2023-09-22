namespace DOCTORLOAN.Models.Departments;

public class Department
{
    public string Code { get; set; }
    public string Name { get; set; }
    public int OrderBy { get; set; }
    public bool IsActive { get; set; }

    //public virtual ICollection<Role> DepartmentRoles { get; } = new HashSet<Role>();
}