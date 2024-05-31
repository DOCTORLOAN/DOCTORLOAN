namespace DOCTORLOAN.Models.Orders;

public class OrderItem
{
    public int OrderId { get; set; }
    public int ProductItemId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice => Quantity * Price; 

    public virtual Order Order { get; set; }
    //public virtual ProductItem ProductItem { get; set; }
}
