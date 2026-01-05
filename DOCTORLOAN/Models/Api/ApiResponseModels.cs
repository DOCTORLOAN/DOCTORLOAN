// <copyright file="ApiResponseModels.cs" company="DOCTORLOAN">
// Copyright (c) DOCTORLOAN. All rights reserved.
// </copyright>

using Newtonsoft.Json;

namespace DOCTORLOAN.Models.Api;

// Generic API Response Wrapper
public class ApiResponse<T>
{
    [JsonProperty("data")]
    public T Data { get; set; }
}

// Product API Response Models
public class ProductListResponse
{
    [JsonProperty("items")]
    public List<ProductResponse> Items { get; set; }
}

public class ProductResponse
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("sku")]
    public string Sku { get; set; }

    [JsonProperty("imageUrl")]
    public string ImageUrl { get; set; }

    [JsonProperty("summary")]
    public string Summary { get; set; }

    [JsonProperty("price")]
    public decimal Price { get; set; }

    [JsonProperty("productItems")]
    public List<ProductItemResponse> ProductItems { get; set; }
}

public class ProductDetailResponse
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("sku")]
    public string Sku { get; set; }

    [JsonProperty("price")]
    public decimal Price { get; set; }

    [JsonProperty("priceDiscount")]
    public decimal PriceDiscount { get; set; }

    [JsonProperty("imageUrl")]
    public string ImageUrl { get; set; }

    [JsonProperty("productMedias")]
    public List<ProductMediaResponse> ProductMedias { get; set; }

    [JsonProperty("productItems")]
    public List<ProductItemResponse> ProductItems { get; set; }

    [JsonProperty("productAttributes")]
    public List<ProductAttributeResponse> ProductAttributes { get; set; }

    [JsonProperty("productDetails")]
    public List<ProductDetailInfoResponse> ProductDetails { get; set; }
}

public class ProductItemResponse
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("sku")]
    public string Sku { get; set; }

    [JsonProperty("price")]
    public decimal Price { get; set; }

    [JsonProperty("productOptions")]
    public List<ProductOptionResponse> ProductOptions { get; set; }
}

public class ProductOptionResponse
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("value")]
    public string Value { get; set; }
}

public class ProductMediaResponse
{
    [JsonProperty("mediaId")]
    public long MediaId { get; set; }

    [JsonProperty("productId")]
    public int ProductId { get; set; }

    [JsonProperty("mediaUrl")]
    public string MediaUrl { get; set; } = string.Empty;

    [JsonProperty("itemCode")]
    public string ItemCode { get; set; } = string.Empty;

    [JsonProperty("orderBy")]
    public int OrderBy { get; set; }
}

public class ProductAttributeResponse
{
    [JsonProperty("productId")]
    public int ProductId { get; set; }

    [JsonProperty("attributeId")]
    public int AttributeId { get; set; }

    [JsonProperty("value")]
    public string Value { get; set; } = string.Empty;
}

public class ProductDetailInfoResponse
{
    [JsonProperty("productId")]
    public int ProductId { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; } = string.Empty;

    [JsonProperty("summary")]
    public string Summary { get; set; } = string.Empty;
}

// Category API Response Models
public class CategoryListResponse
{
    [JsonProperty("items")]
    public required List<CategoryResponse> Items { get; set; }
}

public class CategoryResponse
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("slug")]
    public string Slug { get; set; } = string.Empty;

    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("content")]
    public string Content { get; set; } = string.Empty;

    [JsonProperty("parentId")]
    public int? ParentId { get; set; }
}
