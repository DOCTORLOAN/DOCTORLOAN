namespace DOCTORLOAN.Models.Products;

public class ProductAttribute
{
    public int ProductId { get; set; }
    public int AttributeId { get; set; }
    public string Value { get; set; }
    public virtual Product Product { get; set; }
    public virtual Attribute Attribute { get; set; }
}