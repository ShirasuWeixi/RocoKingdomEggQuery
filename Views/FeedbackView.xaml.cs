using System.Windows.Controls;

namespace RocoKingdomEggQuery.Views
{
    public partial class FeedbackView : UserControl
    {
        public FeedbackView()
        {
            InitializeComponent();
        }

        private void TextBox_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is ViewModels.FeedbackViewModel vm)
            {
                if (!string.IsNullOrEmpty(vm.SpriteName) && vm.Suggestions.Count > 0)
                {
                    vm.ShowSuggestions = true;
                }
            }
        }
    }
}
