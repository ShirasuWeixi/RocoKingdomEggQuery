using System.Text.Json.Serialization;

namespace RocoKingdomEggQuery.Models
{
    public class BayesianModel
    {
        [JsonPropertyName("model_name")]
        public string ModelName { get; set; } = string.Empty;

        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; } = string.Empty;

        [JsonPropertyName("total_samples")]
        public int TotalSamples { get; set; }

        [JsonPropertyName("total_categories")]
        public int TotalCategories { get; set; }

        [JsonPropertyName("category_models")]
        public Dictionary<string, CategoryModel> CategoryModels { get; set; } = new();
    }
}
