﻿namespace DOCTORLOAN.Models.Orders;

public class Order
{
    public string OrderNo { get; set; }
    public int CustomerId { get; set; }
    public decimal SubTotal { get; set; }
    public decimal TotalPrice { get; set; }
    //public OrderStatus Status { get; set; }
    //public PaymentMethod PaymentMethod { get; set; }

    public string FullName { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string AddressLine { get; set; }
    public string Remarks { get; set; }


    public virtual List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    //public virtual Customer Customer { get; set; }
}
