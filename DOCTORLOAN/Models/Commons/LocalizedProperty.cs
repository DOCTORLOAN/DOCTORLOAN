namespace DOCTORLOAN.Models.Commons;
public class LocalizedProperty
{
    public int EntityId { get; set; }
    public int LanguageId { get; set; }
    public string LocaleKeyGroup { get; set; }
    public string LocaleKey { get; set; }
    public string LocaleValue { get; set; }
}