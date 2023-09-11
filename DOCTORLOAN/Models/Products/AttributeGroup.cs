namespace DOCTORLOAN.Models.Products;

public class AttributeGroup
{
    public string Name { get; set; }
    public virtual ICollection<Attribute> Attributes { get; set; }
}
