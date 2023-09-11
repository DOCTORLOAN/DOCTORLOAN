namespace DOCTORLOAN.Models.Products;

public class ProductMedia
{
    public long MediaId { get; set; }
    public int ProductId { get; set; }
    public int OrderBy { get; set; }
    //public StatusEnum Status { get; set; }
    public bool IsDelete { get; set; }
    public int? ProductItemId { get; set; }
    public virtual Product Product { get; set; }
    //public virtual Media Media { get; set; }
}
