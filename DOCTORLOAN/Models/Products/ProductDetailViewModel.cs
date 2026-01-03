// <copyright file="ProductDetailViewModel.cs" company="DOCTORLOAN">
// Copyright (c) DOCTORLOAN. All rights reserved.
// </copyright>

namespace DOCTORLOAN.Models.Products;

public class ProductDetailViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Sku { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public string ImageUrl { get; set; } = string.Empty;

    public List<ProductMediaViewModel> ProductMedias { get; set; } = new List<ProductMediaViewModel>();

    public List<ProductItemViewModel> ProductItems { get; set; } = new List<ProductItemViewModel>();

    public List<ProductAttributeViewModel> ProductAttributes { get; set; } = new List<ProductAttributeViewModel>();

    public List<ProductDetailInfoViewModel> ProductDetails { get; set; } = new List<ProductDetailInfoViewModel>();

    public List<ProductViewModel> RelatedProducts { get; set; } = new List<ProductViewModel>();
}

public class ProductMediaViewModel
{
    public long MediaId { get; set; }

    public int ProductId { get; set; }

    public string MediaUrl { get; set; } = string.Empty;

    public string ItemCode { get; set; } = string.Empty;

    public int OrderBy { get; set; }
}

public class ProductAttributeViewModel
{
    public int ProductId { get; set; }

    public int AttributeId { get; set; }

    public string Value { get; set; } = string.Empty;
}

public class ProductDetailInfoViewModel
{
    public int ProductId { get; set; }

    public string Description { get; set; } = string.Empty;

    public string Summary { get; set; } = string.Empty;
}
