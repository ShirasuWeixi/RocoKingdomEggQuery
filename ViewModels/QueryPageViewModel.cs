using System.Collections.ObjectModel;
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
        private FeedbackViewModel _feedbackViewModel = new();

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

        public FeedbackViewModel FeedbackViewModel
        {
            get => _feedbackViewModel;
            set => SetProperty(ref _feedbackViewModel, value);
        }

        public RelayCommand QueryCommand { get; }
        public RelayCommand FeedbackCommand { get; }

        public QueryPageViewModel()
        {
            _modelService = new BayesianModelService();
            QueryCommand = new RelayCommand(async () => await OnQueryAsync(), CanExecuteQuery);
            FeedbackCommand = new RelayCommand(OnFeedback, CanExecuteFeedback);
        }

        private bool CanExecuteQuery()
        {
            if (IsLoading) return false;
            return double.TryParse(SizeInput, out _) && double.TryParse(MassInput, out _);
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

                if (!double.TryParse(SizeInput, out double size) ||
                    !double.TryParse(MassInput, out double mass))
                {
                    StatusMessage = "输入格式错误，请输入有效的数字";
                    return;
                }

                StatusMessage = "正在计算后验概率...";
                var allResults = _modelService.CalculatePosteriorProbabilities(_model, size, mass);
                
                var filteredResults = allResults.Where(r => r.PosteriorProbability >= ProbabilityThreshold).ToList();
<<<<<<< Updated upstream
                
                foreach (var result in filteredResults)
                {
                    Results.Add(result);
                }

                TopResult = filteredResults.FirstOrDefault();
                
=======

            foreach (var result in filteredResults)
            {
                Results.Add(result);
            }

            TopResult = filteredResults.FirstOrDefault();
            
            // 更新反馈视图中的精灵名称列表
            FeedbackViewModel.SetSpriteNamesFromResults(filteredResults.Select(r => r.Name));

>>>>>>> Stashed changes
                if (TopResult != null)
                {
                    TopResult.IsTopResult = true;
                }
                
                StatusMessage = Results.Count > 0 
                    ? $"查询完成，找到 {Results.Count} 个匹配结果" 
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
            FeedbackViewModel.Show(SizeInput, MassInput);
        }
    }
}