namespace DOCTORLOAN.Models.Banks;

public class Branch
{
    public string Name { get; set; }
    public string Code { get; set; }
    public bool IsDelete { get; set; }

    public virtual ICollection<BankBranch> BankBranchs { get; set; } = new List<BankBranch>();
}
