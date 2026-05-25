namespace RocoKingdomEggQuery.Services
{
    public interface INavigationService
    {
        string CurrentPage { get; }
        void NavigateTo(string pageName);
        event Action<string>? NavigationChanged;
    }
}