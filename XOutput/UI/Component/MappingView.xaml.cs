using System.Windows;
using System.Windows.Controls;

namespace XOutput.UI.Component
{
    /// <summary>
    /// Interaction logic for MappingView.xaml
    /// </summary>
    public partial class MappingView : UserControl, IViewBase<MappingViewModel, MappingModel>
    {
        public MappingViewModel ViewModel { get; private set; }

        public MappingView(MappingViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = viewModel;
            InitializeComponent();
        }

        public void CleanUp()
        {
            ViewModel.CleanUp();
        }

        public void Refresh()
        {
            ViewModel.Refresh();
        }

        private void ConfigureClick(object sender, RoutedEventArgs e)
        {
            ViewModel.Configure();
            Refresh();
        }

        private void InvertClick(object sender, RoutedEventArgs e)
        {
            ViewModel.Invert();
        }
    }
}
