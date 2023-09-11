namespace DOCTORLOAN.Models.News;

public class NewsTagsMapping
{
    public int NewsItemId { get; set; }
    public int NewsTagId { get; set; }
    public virtual NewsItem NewsItem { get; set; }
    public virtual NewsTag NewsTag { get; set; }
}
