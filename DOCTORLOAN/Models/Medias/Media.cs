namespace DOCTORLOAN.Models.Medias;

public class Media
{
    public string OriginalName { get; set; }

    public string Name { get; set; }

    public string ContentType { get; set; }

    public string Extention { get; set; }

    public string Path { get; set; }

    //public MediaType Type { get; set; }

    //public MediaStatus Status { get; set; }

    public bool HasStorage { get; set; } = true;
}
