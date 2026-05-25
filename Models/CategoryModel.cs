using System.Text.Json.Serialization;

namespace RocoKingdomEggQuery.Models
{
    public class CategoryModel
    {
        [JsonPropertyName("ak")]
        public double Ak { get; set; }

        [JsonPropertyName("bk")]
        public double Bk { get; set; }

        [JsonPropertyName("sigma_k")]
        public double SigmaK { get; set; }

        [JsonPropertyName("prior_probability")]
        public double PriorProbability { get; set; }

        [JsonPropertyName("min_size")]
        public double MinSize { get; set; }

        [JsonPropertyName("max_size")]
        public double MaxSize { get; set; }

        [JsonPropertyName("sample_count")]
        public int SampleCount { get; set; }

        [JsonIgnore]
        public string Name { get; set; } = string.Empty;

        [JsonIgnore]
        public double PosteriorProbability { get; set; }

        [JsonIgnore]
        public bool IsTopResult { get; set; }
    }
}
