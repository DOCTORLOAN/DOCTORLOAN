// <copyright file="Order.cs" company="DOCTORLOAN">
// Copyright (c) DOCTORLOAN. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace DOCTORLOAN.Models.Orders;

public class Order
{
    public int CustomerId { get; set; }

    [Range(0, double.MaxValue)]
    public decimal SubTotal { get; set; }

    [Range(0, double.MaxValue)]
    public decimal TotalPrice { get; set; }

    [Required]
    public PaymentMethod PaymentMethod { get; set; }

    [Required]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [Phone]
    [StringLength(20)]
    public string Phone { get; set; } = string.Empty;

    [EmailAddress]
    [StringLength(256)]
    public string? Email { get; set; }

    [Required]
    [StringLength(250)]
    public string AddressLine { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Remarks { get; set; }

    public virtual List<ListItem> ListItem { get; set; } = new();
}

public class ListItem
{
    [Required]
    public int ProductId { get; set; }

    [Required]
    public int ProductItemId { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [StringLength(100)]
    public string? ProductSku { get; set; }

    [StringLength(100)]
    public string? OptionName { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    [Range(1, 1000)]
    public int Quantity { get; set; }

    [Range(0, double.MaxValue)]
    public decimal TotalPrice { get; set; }
}

public enum PaymentMethod
{
    Cash = 1,
    Payment_On_Delivery = 2,
    Payoo = 3,
}
