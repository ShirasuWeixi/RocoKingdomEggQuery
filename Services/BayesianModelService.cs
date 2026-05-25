using System.Text.Json;
using RocoKingdomEggQuery.Models;
using System.IO;

namespace RocoKingdomEggQuery.Services
{
    public class BayesianModelService
    {
        private const string ModelFileName = "bayesian_model.json";
        private BayesianModel? _cachedModel;

        public async Task<BayesianModel> LoadModelAsync()
        {
            if (_cachedModel != null)
                return _cachedModel;

            string modelPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", ModelFileName);

            if (!File.Exists(modelPath))
            {
                modelPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ModelFileName);
                if (!File.Exists(modelPath))
                {
                    throw new FileNotFoundException($"未找到模型文件: {ModelFileName}");
                }
            }

            string jsonContent = await File.ReadAllTextAsync(modelPath);
            _cachedModel = JsonSerializer.Deserialize<BayesianModel>(jsonContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (_cachedModel != null)
            {
                foreach (var kvp in _cachedModel.CategoryModels)
                {
                    kvp.Value.Name = kvp.Key;
                }
            }

            return _cachedModel ?? new BayesianModel();
        }

        public List<CategoryModel> CalculatePosteriorProbabilities(BayesianModel model, double size, double mass)
        {
            var results = new List<CategoryModel>();
            double totalProbability = 0;

            foreach (var category in model.CategoryModels.Values)
            {
                double pXGivenK = CalculatePXGivenK(category, size);
                double normalPdf = CalculateNormalPdf(mass, category.Ak * size + category.Bk, category.SigmaK);
                double jointProbability = category.PriorProbability * normalPdf * pXGivenK;

                var resultCategory = new CategoryModel
                {
                    Name = category.Name,
                    Ak = category.Ak,
                    Bk = category.Bk,
                    SigmaK = category.SigmaK,
                    PriorProbability = category.PriorProbability,
                    MinSize = category.MinSize,
                    MaxSize = category.MaxSize,
                    SampleCount = category.SampleCount,
                    PosteriorProbability = jointProbability
                };

                results.Add(resultCategory);
                totalProbability += jointProbability;
            }

            if (totalProbability > 0)
            {
                foreach (var category in results)
                {
                    category.PosteriorProbability /= totalProbability;
                }
            }

            return results.OrderByDescending(c => c.PosteriorProbability).ToList();
        }

        private double CalculatePXGivenK(CategoryModel category, double size)
        {
            double meanSize = (category.MinSize + category.MaxSize) / 2;
            double stdSize = (category.MaxSize - category.MinSize) / 4;

            if (stdSize <= 0)
                stdSize = 0.1;

            return CalculateNormalPdf(size, meanSize, stdSize);
        }

        private double CalculateNormalPdf(double x, double mean, double stdDev)
        {
            if (stdDev <= 0)
                return 1;

            double exponent = -0.5 * Math.Pow((x - mean) / stdDev, 2);
            return Math.Exp(exponent) / (stdDev * Math.Sqrt(2 * Math.PI));
        }
    }
}
