namespace DOCTORLOAN.Models.Contents;

public class Content
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    //public StatusEnum Status { get; set; }
    public long? MediaId { get; set; }

    //public ContentTypeEnum Type { get; set; }
    //public virtual Media Media { get; set; }
}