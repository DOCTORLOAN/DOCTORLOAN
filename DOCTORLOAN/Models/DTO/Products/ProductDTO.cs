﻿namespace DOCTORLOAN.Models.DTO.Products;

 public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Sku { get; set; }
    public int Status { get; set; }
    public int BrandId { get; set; }
    public decimal PriceDiscount { get; set; }
    public decimal Price { get; set; }
    public int? Quantity { get; set; }
    public List<ProductItemDto> ProductItems { get; set; } = new();
    public List<ProductAttributeDto> ProductAttributes { get; set; } = new();
    public List<ProductDetailDto> ProductDetails { get; set; } = new();
    public List<ProductMediaDto> ProductMedias { get; set; } = new();
    public List<ProductCategoryDto> ProductCategories { get; set; } = new();
    public List<int> CategoryIds { get; set; } = new();
}
public class ProductItemDto
{
    public int Id { get; set; }
    public int BrandId { get; set; }
    public string? Name { get; set; }
    public string? Sku { get; set; }
    public decimal PriceDiscount { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public List<ProductOptionDto> ProductOptions { get; set; } = new List<ProductOptionDto>();

}
public class ProductOptionDto
{
    public int OptionGroupId { get; set; }
    public string Name { get; set; }
    public string DisplayValue { get; set; }
}

public class ProductAttributeDto
{
    public int AttributeId { get; set; }
    public string Value { get; set; }
}
public class ProductDetailDto
{

    public int LanguageId { get; set; }
    public string? Description { get; set; }
    public string? Summary { get; set; }
    public string? MetadataKeyword { get; set; }
    public string? MetadataTitle { get; set; }
    public string? MetadataDesc { get; set; }
}
public class ProductMediaDto
{
    public long MediaId { get; set; }
    public int OrderBy { get; set; }
    public string? MediaUrl { get; set; }
    public string ItemCode { get; set; } = string.Empty;
}
public class ProductCategoryDto
{
    public int CategoryId { get; set; }
    public string? Name { get; set; }
}

public class ProductFilterResultDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public List<string> CategoryNames { get; set; } = new List<string>();
    public int Status { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public int AvaiableStock { get; set; }
    public int Stock { get; set; }
    public DateTimeOffset? LastModified { get; set; }
    public List<ProductItemDto> ProductItems { get; set; } = new List<ProductItemDto>();
}
