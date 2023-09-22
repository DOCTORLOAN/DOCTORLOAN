namespace DOCTORLOAN.Models.Products;

public class Attribute
{
    public string Name { get; set; }
    public int GroupId { get; set; }
    public virtual AttributeGroup AttributeGroup { get; set; }
}
