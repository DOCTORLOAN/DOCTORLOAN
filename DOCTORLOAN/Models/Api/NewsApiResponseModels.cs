using Newtonsoft.Json;

namespace DOCTORLOAN.Models.Api;

// News API Response Models
public class NewsListResponse
{
    [JsonProperty("items")]
    public List<NewsItemResponse> Items { get; set; } = new List<NewsItemResponse>();
}

public class NewsItemResponse
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; } = string.Empty;

    [JsonProperty("imageUrl")]
    public string ImageUrl { get; set; } = string.Empty;

    [JsonProperty("summary")]
    public string? Summary { get; set; }

    [JsonProperty("content")]
    public string? Content { get; set; }

    [JsonProperty("slug")]
    public string? Slug { get; set; }
}

