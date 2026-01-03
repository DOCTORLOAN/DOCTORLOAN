// <copyright file="ApiConstants.cs" company="DOCTORLOAN">
// Copyright (c) DOCTORLOAN. All rights reserved.
// </copyright>

namespace DOCTORLOAN.Constants;

/// <summary>
/// Constants cho API base URLs và endpoints.
/// </summary>
public static class ApiConstants
{
    /// <summary>
    /// Base URL cho tất cả các API modules.
    /// </summary>
    private const string ApiBaseUrl = "https://doctorloan-api.giathaidoctorloan.vn/api";

    /// <summary>
    /// Product Module API Base URL.
    /// </summary>
    public const string ProductModuleBaseUrl = $"{ApiBaseUrl}/product-module";

    /// <summary>
    /// Order Module API Base URL.
    /// </summary>
    public const string OrderModuleBaseUrl = $"{ApiBaseUrl}/order-module";

    /// <summary>
    /// Booking Module API Base URL.
    /// </summary>
    public const string BookingModuleBaseUrl = $"{ApiBaseUrl}/booking-module";

    /// <summary>
    /// News Module API Base URL.
    /// </summary>
    public const string NewsModuleBaseUrl = $"{ApiBaseUrl}/news-module";

    // ===== Product Module Endpoints =====

    /// <summary>
    /// Filter products endpoint.
    /// </summary>
    public const string ProductFilterProducts = $"{ProductModuleBaseUrl}/Product/FilterProducts";

    /// <summary>
    /// Get product by ID endpoint.
    /// </summary>
    public const string ProductGetProduct = $"{ProductModuleBaseUrl}/Product/GetProduct";

    /// <summary>
    /// Filter categories endpoint.
    /// </summary>
    public const string CategoryFilterCategories = $"{ProductModuleBaseUrl}/Category/FilterCategories";

    // ===== Order Module Endpoints =====

    /// <summary>
    /// Create order endpoint.
    /// </summary>
    public const string OrderCreate = $"{OrderModuleBaseUrl}/Order/create";

    /// <summary>
    /// Payoo callback endpoint.
    /// </summary>
    public const string OrderCallbackPayoo = $"{OrderModuleBaseUrl}/Order/callback-payoo";

    // ===== Booking Module Endpoints =====

    /// <summary>
    /// Create booking endpoint.
    /// </summary>
    public const string BookingCreate = $"{BookingModuleBaseUrl}/Booking/create";

    // ===== News Module Endpoints =====

    /// <summary>
    /// Filter news categories endpoint.
    /// </summary>
    public const string NewsCategoryFilterCategories = $"{NewsModuleBaseUrl}/NewsCategory/FilterCategories";

    /// <summary>
    /// Get news by ID endpoint.
    /// </summary>
    public const string NewsItemGetNewsById = $"{NewsModuleBaseUrl}/NewsItem/GetNewById";

    /// <summary>
    /// Filter news items endpoint.
    /// </summary>
    public const string NewsItemFilterNews = $"{NewsModuleBaseUrl}/NewsItem/FilterNews";
}
