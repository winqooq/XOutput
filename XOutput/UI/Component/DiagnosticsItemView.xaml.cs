using System.Windows.Controls;

namespace XOutput.UI.Component
{
    /// <summary>
    /// Interaction logic for MappingView.xaml
    /// </summary>
    public partial class DiagnosticsItemView : UserControl, IViewBase<DiagnosticsItemViewModel, DiagnosticsItemModel>
    {
        public DiagnosticsItemViewModel ViewModel { get; private set; }

        public DiagnosticsItemView(DiagnosticsItemViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = viewModel;
            InitializeComponent();
        }

        public void CleanUp()
        {
            ViewModel.CleanUp();
        }
    }
}
