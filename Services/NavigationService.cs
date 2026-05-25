namespace RocoKingdomEggQuery.Services
{
    public class NavigationService : INavigationService
    {
        private string _currentPage = string.Empty;

        public string CurrentPage
        {
            get => _currentPage;
            private set
            {
                if (_currentPage != value)
                {
                    _currentPage = value;
                    NavigationChanged?.Invoke(_currentPage);
                }
            }
        }

        public event Action<string>? NavigationChanged;

        public void NavigateTo(string pageName)
        {
            if (string.IsNullOrWhiteSpace(pageName))
                return;

            CurrentPage = pageName;
        }
    }
}