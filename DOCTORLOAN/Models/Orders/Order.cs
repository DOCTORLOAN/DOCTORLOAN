using DOCTORLOAN.Models.Products;

namespace DOCTORLOAN.Models.Orders;

public class Order
{
    public int CustomerId { get; set; }
    public decimal SubTotal { get; set; }
    public decimal TotalPrice { get; set; }
    public PaymentMethod PaymentMethod { get; set; }

    public string FullName { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string AddressLine { get; set; }
    public string Remarks { get; set; }
    public virtual List<ListItem> ListItem { get; set; } = new List<ListItem>();
}

public class ListItem
{
    public int ProductItemId { get; set; }
    public string Name { get; set; }
    public string ProductSku { get; set; }
    public string OptionName { get; set; }
    public decimal Price { get; set; }
    public string Quantity { get; set; }
    public decimal TotalPrice { get; set; }
}

public enum PaymentMethod
{
    Cash = 1,
    Payment_On_Delivery = 2,
}