using System.Windows;
using System.Windows.Controls;
using XOutput.Tools;

namespace XOutput.UI.Component
{
    /// <summary>
    /// Interaction logic for InputView.xaml
    /// </summary>
    public partial class InputView : UserControl, IViewBase<InputViewModel, InputModel>
    {
        public InputViewModel ViewModel { get; private set; }

        [ResolverMethod(Scope.Prototype)]
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
