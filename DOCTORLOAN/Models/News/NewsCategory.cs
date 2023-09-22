namespace DOCTORLOAN.Models.News;

public class NewsCategory
{
    public int? ParentId { get; set; }
    public string Name { get; set; }
    //public StatusEnum Status { get; set; }
    public string Slug { get; set; }
    public string Description { get; set; }
    public int Sort { get; set; }
    public bool IsDeleted { get; set; }
}
