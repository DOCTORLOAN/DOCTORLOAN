namespace DOCTORLOAN.Models.Products;

public class ProductItem
{
    public int Id { get; set; }
    public int BrandId { get; set; }
    public string? Name { get; set; }
    public string? Sku { get; set; }
    public decimal PriceDiscount { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public List<ProductOption> ProductOptions { get; set; } = new List<ProductOption>();
}
