using System.Windows;
using RocoKingdomEggQuery.Services;
using RocoKingdomEggQuery.ViewModels;

namespace RocoKingdomEggQuery
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var navigationService = new NavigationService();
            var mainViewModel = new MainViewModel(navigationService);
            DataContext = mainViewModel;
        }
    }
}