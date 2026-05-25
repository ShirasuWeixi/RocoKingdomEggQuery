using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using RocoKingdomEggQuery.Commands;
using RocoKingdomEggQuery.Models;
using RocoKingdomEggQuery.Services;

namespace RocoKingdomEggQuery.ViewModels
{
    public class QueryPageViewModel : BaseViewModel
    {
        private readonly BayesianModelService _modelService;
        private BayesianModel? _model;
        private string _sizeInput = string.Empty;
        private string _massInput = string.Empty;
        private double _probabilityThreshold = 0.01;
        private ObservableCollection<CategoryModel> _results = new();
        private CategoryModel? _topResult;
        private bool _isLoading;
        private string _statusMessage = "准备就绪";

        public string SizeInput
        {
            get => _sizeInput;
            set
            {
                SetProperty(ref _sizeInput, value);
                QueryCommand.RaiseCanExecuteChanged();
            }
        }

        public string MassInput
        {
            get => _massInput;
            set
            {
                SetProperty(ref _massInput, value);
                QueryCommand.RaiseCanExecuteChanged();
            }
        }

        public double ProbabilityThreshold
        {
            get => _probabilityThreshold;
            set => SetProperty(ref _probabilityThreshold, value);
        }

        public ObservableCollection<CategoryModel> Results
        {
            get => _results;
            set => SetProperty(ref _results, value);
        }

        public CategoryModel? TopResult
        {
            get => _topResult;
            set => SetProperty(ref _topResult, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                SetProperty(ref _isLoading, value);
                QueryCommand.RaiseCanExecuteChanged();
            }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public RelayCommand QueryCommand { get; }
        public RelayCommand FeedbackCommand { get; }

        public QueryPageViewModel()
        {
            _modelService = new BayesianModelService();
            _modelService.FilteredSpriteLogged += OnFilteredSpriteLogged;
            QueryCommand = new RelayCommand(async () => await OnQueryAsync(), CanExecuteQuery);
            FeedbackCommand = new RelayCommand(OnFeedback, CanExecuteFeedback);
        }

        private bool CanExecuteQuery()
        {
            if (IsLoading) return false;
            return IsValidSizeInput(SizeInput) && double.TryParse(MassInput, out _);
        }

        private bool IsValidSizeInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            if (double.TryParse(input.Trim(), out _))
                return true;

            var rangePattern = @"^([+-]?\d*\.?\d+)\s*[-~]\s*([+-]?\d*\.?\d+)$";
            var match = Regex.Match(input.Trim(), rangePattern);
            if (match.Success)
            {
                return double.TryParse(match.Groups[1].Value, out double minVal) &&
                       double.TryParse(match.Groups[2].Value, out double maxVal) &&
                       minVal <= maxVal;
            }

            return false;
        }

        private (double minSize, double maxSize) ParseSizeInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return (0, 0);

            input = input.Trim();

            if (double.TryParse(input, out double singleValue))
                return (singleValue, singleValue);

            var rangePattern = @"^([+-]?\d*\.?\d+)\s*[-~]\s*([+-]?\d*\.?\d+)$";
            var match = Regex.Match(input, rangePattern);
            if (match.Success)
            {
                double minVal = double.Parse(match.Groups[1].Value);
                double maxVal = double.Parse(match.Groups[2].Value);
                return (Math.Min(minVal, maxVal), Math.Max(minVal, maxVal));
            }

            return (0, 0);
        }

        private bool CanExecuteFeedback()
        {
            return true;
        }

        private async Task OnQueryAsync()
        {
            IsLoading = true;
            StatusMessage = "正在加载模型...";
            Results.Clear();
            TopResult = null;

            try
            {
                if (_model == null)
                {
                    _model = await _modelService.LoadModelAsync();
                }

                var (minSize, maxSize) = ParseSizeInput(SizeInput);

                if (!double.TryParse(MassInput, out double mass))
                {
                    StatusMessage = "质量输入格式错误，请输入有效的数字";
                    return;
                }

                StatusMessage = "正在计算后验概率...";
                var allResults = _modelService.CalculatePosteriorProbabilities(_model, minSize, maxSize, mass);

                var filteredResults = allResults.Where(r => r.PosteriorProbability >= ProbabilityThreshold).ToList();

                foreach (var result in filteredResults)
                {
                    Results.Add(result);
                }

                TopResult = filteredResults.FirstOrDefault();

                if (TopResult != null)
                {
                    TopResult.IsTopResult = true;
                }

                int filteredCount = _modelService.FilteredSpriteCount;
                string filterInfo = filteredCount > 0 ? $" (已过滤 {filteredCount} 个尺寸范围不匹配的精灵)" : "";

                StatusMessage = Results.Count > 0
                    ? $"查询完成，找到 {Results.Count} 个匹配结果{filterInfo}"
                    : "没有找到符合概率阈值的结果";
            }
            catch (Exception ex)
            {
                StatusMessage = $"查询失败: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void OnFeedback()
        {
            StatusMessage = "感谢您的反馈！";
        }

        private void OnFilteredSpriteLogged(object? sender, string message)
        {
            System.Diagnostics.Debug.WriteLine(message);
        }
    }
}