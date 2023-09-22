namespace DOCTORLOAN.Models.Products;

public class ProductOption
{
    public int ProductItemId { get; set; }
    public int OptionGroupId { get; set; }
    public string Name { get; set; }
    public string DisplayValue { get; set; }
    public virtual ProductItem ProductItem { get; set; }
    public virtual ProductOptionGroup OptionGroup { get; set; }
}
