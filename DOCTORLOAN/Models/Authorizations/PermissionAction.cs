namespace DOCTORLOAN.Models.Authorizations;

public class PermissionAction
{
    public int ModuleId { get; set; }
    public int ActionId { get; set; }
    public bool IsDelete { get; set; }

    public virtual ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
}
