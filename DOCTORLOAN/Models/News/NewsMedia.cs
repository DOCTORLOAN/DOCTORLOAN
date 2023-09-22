namespace DOCTORLOAN.Models.News;

public class NewsMedia
{
    public bool IsThumb { get; set; }
    public int NewsId { get; set; }
    public long MediaId { get; set; }
    public int OrderBy { get; set; }
    //public virtual Media Media { get; set; }
    public virtual NewsItem NewsItem { get; set; }
}
