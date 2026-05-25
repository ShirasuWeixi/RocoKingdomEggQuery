using RocoKingdomEggQuery.Commands;
using RocoKingdomEggQuery.Services;

namespace RocoKingdomEggQuery.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private string _currentPage = string.Empty;
        private string _currentView = "查询";

        public string CurrentPage
        {
            get => _currentPage;
            set => SetProperty(ref _currentPage, value);
        }

        public string CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        public RelayCommand NavigateToQueryCommand { get; }
        public RelayCommand NavigateToSettingsCommand { get; }

        public MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            _navigationService.NavigationChanged += OnNavigationChanged;

            NavigateToQueryCommand = new RelayCommand(() => NavigateTo("Query"));
            NavigateToSettingsCommand = new RelayCommand(() => NavigateTo("Settings"));

            NavigateTo("Query");
        }

        private void NavigateTo(string pageName)
        {
            _navigationService.NavigateTo(pageName);
        }

        private void OnNavigationChanged(string pageName)
        {
            CurrentPage = pageName;
            CurrentView = pageName switch
            {
                "Query" => "查询",
                "Settings" => "设置",
                _ => CurrentView
            };
        }
    }
}