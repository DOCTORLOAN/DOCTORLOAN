using DOCTORLOAN.Models.Products;

namespace DOCTORLOAN.Models.Orders;

public class PaymentViewModel
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public ProductDetailViewModel Product { get; set; } = new ProductDetailViewModel();
    public int SelectedProductItemId { get; set; }
    public decimal SubTotal { get; set; }
    public decimal ShippingFee { get; set; }
    public decimal TotalPrice { get; set; }
}

