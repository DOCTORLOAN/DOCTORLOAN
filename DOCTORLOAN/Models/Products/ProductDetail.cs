namespace DOCTORLOAN.Models.Products;

public class ProductDetail
{
    public int ProductId { get; set; }
    public int LanguageId { get; set; }
    public string Description { get; set; }
    public string Summary { get; set; }
    public string MetadataKeyword { get; set; }
    public string MetadataTitle { get; set; }
    public string MetadataDesc { get; set; }
    public virtual Product Product { get; set; }
}
