namespace DOCTORLOAN.Models.Products;

public class ProductListViewModel
{
    public List<ProductViewModel> Products { get; set; } = new List<ProductViewModel>();
}

public class ProductViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public List<ProductItemViewModel> ProductItems { get; set; } = new List<ProductItemViewModel>();
}

public class ProductItemViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public List<ProductOptionViewModel> ProductOptions { get; set; } = new List<ProductOptionViewModel>();
}

public class ProductOptionViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}

