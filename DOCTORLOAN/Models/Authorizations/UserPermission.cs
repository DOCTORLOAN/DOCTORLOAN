namespace DOCTORLOAN.Models.Authorizations;

public class UserPermission
{
    public int UserId { get; set; }
    public int PermissionActionId { get; set; }


    //public virtual User User { get; set; }
    public virtual PermissionAction PermissionAction { get; set; }
}
