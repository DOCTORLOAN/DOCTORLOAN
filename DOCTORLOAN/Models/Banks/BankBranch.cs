namespace DOCTORLOAN.Models.Banks;

public class BankBranch
{
    public int BankId { get; set; }
    public int BranchId { get; set; }
    public string Code { get; set; }
    public bool IsDelete { get; set; }

    public virtual Bank Bank { get; set; }
    public virtual Branch Branch { get; set; }

    //public virtual ICollection<UserBankBranch> UserBankBranchs { get; set; } = new List<UserBankBranch>();
}
