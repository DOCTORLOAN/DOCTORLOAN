namespace DOCTORLOAN.Models.Products;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Sku { get; set; }
    public int Status { get; set; }
    public string BrandName { get; set; }
    public decimal PriceDiscount { get; set; }
    public decimal Price { get; set; }
    public int? Quantity { get; set; }
    public List<ProductItem> ProductItems { get; set; }
    public List<ProductAttribute> ProductAttributes { get; set; }
    public List<ProductDetail> ProductDetails { get; set; }
    public string ImgUrl { get; set; }
    public List<ProductMedia> ProductMedias { get; set; }
    public string CategorieName { get; set; }
    public List<ProductCategory> ProductCategories { get; set; }
    public List<int> CategoryIds { get; set; }
}
