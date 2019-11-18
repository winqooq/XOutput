using System.Windows;
using System.Windows.Controls;

namespace XOutput.UI.Component
{
    /// <summary>
    /// Interaction logic for InputView.xaml
    /// </summary>
    public partial class InputView : UserControl, IViewBase<InputViewModel, InputModel>
    {
        public InputViewModel ViewModel { get; private set; }

        public InputView(InputViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = viewModel;
            InitializeComponent();
        }

        public void CleanUp()
        {
            ViewModel.CleanUp();
        }

        private void OpenClick(object sender, RoutedEventArgs e)
        {
            ViewModel.Edit();
        }
    }
}
