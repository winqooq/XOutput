using System.Windows.Controls;
using XOutput.Devices;

namespace XOutput.UI.Component
{
    /// <summary>
    /// Interaction logic for AxisView.xaml
    /// </summary>
    public partial class AxisView : UserControl, IUpdatableView, IViewBase<AxisViewModel, AxisModel>
    {
        public AxisViewModel ViewModel { get; private set; }

        public AxisView(AxisViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = viewModel;
            InitializeComponent();
        }

        public void CleanUp()
        {
            ViewModel.CleanUp();
        }

        public void UpdateValues(IDevice device)
        {
            ViewModel.UpdateValues(device);
        }
    }
}
