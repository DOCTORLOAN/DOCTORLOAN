namespace DOCTORLOAN.Models.News;

public class NewsItemDetail
{
    public int NewsId { get; set; }
    public int LanguageId { get; set; }
    public string Title { get; set; }
    public string Short { get; set; }
    public string Full { get; set; }
    public string MetaTitle { get; set; }
    public string MetaKeyword { get; set; }
    public string MetaDescription { get; set; }
    public virtual NewsItem NewsItem { get; set; }
}
