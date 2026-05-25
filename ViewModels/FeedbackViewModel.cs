using System.Collections.ObjectModel;
using System.Windows.Input;
using RocoKingdomEggQuery.Commands;
using RocoKingdomEggQuery.Models;
using RocoKingdomEggQuery.Services;

namespace RocoKingdomEggQuery.ViewModels
{
    public class FeedbackViewModel : BaseViewModel
    {
        private string _sizeData = string.Empty;
        private string _massData = string.Empty;
        private string _spriteName = string.Empty;
        private bool _isVisible = false;
        private bool _showSuggestions = false;
        private ObservableCollection<string> _suggestions = new();
        private string? _selectedSuggestion = null;
        private CancellationTokenSource? _debounceCts = null;
        private List<string> _allSpriteNames = new();

        public string SizeData
        {
            get => _sizeData;
            set => SetProperty(ref _sizeData, value);
        }

        public string MassData
        {
            get => _massData;
            set => SetProperty(ref _massData, value);
        }

        public string SpriteName
        {
            get => _spriteName;
            set
            {
                if (SetProperty(ref _spriteName, value))
                {
                    DebounceGetSuggestions(value);
                }
            }
        }

        public bool IsVisible
        {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);
        }

        public bool ShowSuggestions
        {
            get => _showSuggestions;
            set => SetProperty(ref _showSuggestions, value);
        }

        public ObservableCollection<string> Suggestions
        {
            get => _suggestions;
            set => SetProperty(ref _suggestions, value);
        }

        public string? SelectedSuggestion
        {
            get => _selectedSuggestion;
            set
            {
                if (SetProperty(ref _selectedSuggestion, value) && !string.IsNullOrEmpty(value))
                {
                    SpriteName = value;
                    ShowSuggestions = false;
                }
            }
        }

        public ICommand SubmitCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand SelectSuggestionCommand { get; }

        public FeedbackViewModel()
        {
            SubmitCommand = new RelayCommand(OnSubmit, CanSubmit);
            CancelCommand = new RelayCommand(OnCancel);
            SelectSuggestionCommand = new RelayCommand<string>(OnSelectSuggestion);
            InitializeSampleSprites();
        }

        private void InitializeSampleSprites()
        {
            _allSpriteNames = new List<string>
            {
                "火花", "水蓝蓝", "喵喵", "蹦蹦种子", "蒲公英", "可爱猿",
                "雪娃娃", "音速犬", "罗隐", "瞌睡王", "格兰球", "卡卡虫",
                "利灯鱼", "神圣玄武", "邪恶玄武", "冰龙王", "萌王", "翼王",
                "草王", "幽王", "翼王", "精灵王", "皇家圣光迪莫", "圣光迪莫",
                "毁灭伯爵", "圣诞麋鹿王", "红毛小Q", "酷拉", "冰翼邪神",
                "上古战龙", "幻影霸主", "飞翅连击", "超级瓦斯", "布莱克岩",
                "罗隐", "瞌睡王", "音速犬", "格兰球", "卡瓦重", "少林呱呱",
                "富贵呱呱", "逍遥呱呱", "狼烟之魂", "狼烟圣魂"
            };
        }

        public void SetSpriteNamesFromResults(IEnumerable<string> names)
        {
            _allSpriteNames = names.ToList();
        }

        private void DebounceGetSuggestions(string input)
        {
            _debounceCts?.Cancel();
            _debounceCts = new CancellationTokenSource();
            Task.Delay(300, _debounceCts.Token).ContinueWith(t =>
            {
                if (!t.IsCanceled)
                {
                    GetSuggestions(input);
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void GetSuggestions(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                Suggestions.Clear();
                ShowSuggestions = false;
                return;
            }

            var results = _allSpriteNames
                .Where(name => 
                    name.Contains(input, StringComparison.OrdinalIgnoreCase) ||
                    PinyinHelperService.GetFirstPinyin(name).Contains(input.ToLower()))
                .Take(3)
                .ToList();

            Suggestions.Clear();
            foreach (var suggestion in results)
            {
                Suggestions.Add(suggestion);
            }

            ShowSuggestions = Suggestions.Count > 0;
        }

        private void OnSelectSuggestion(string? suggestion)
        {
            if (!string.IsNullOrEmpty(suggestion))
            {
                SelectedSuggestion = suggestion;
            }
        }

        private bool CanSubmit()
        {
            return !string.IsNullOrWhiteSpace(SpriteName);
        }

        private void OnSubmit()
        {
            System.Windows.MessageBox.Show("反馈已记录，功能即将上线！", "提示",
                System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            OnCancel();
        }

        private void OnCancel()
        {
            IsVisible = false;
            SpriteName = string.Empty;
            ShowSuggestions = false;
            Suggestions.Clear();
        }

        public void Show(string sizeData, string massData)
        {
            SizeData = sizeData;
            MassData = massData;
            IsVisible = true;
        }
    }
}
