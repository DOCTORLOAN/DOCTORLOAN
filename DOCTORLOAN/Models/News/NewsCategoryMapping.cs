namespace DOCTORLOAN.Models.News;

public class NewsCategoryMapping
{
    public int NewsItemId { get; set; }
    public int NewsCategoryId { get; set; }
    public virtual NewsCategory NewsCategory { get; set; }
    public virtual NewsItem NewsItem { get; set; }
}
