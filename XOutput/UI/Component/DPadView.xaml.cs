using System.Windows.Controls;
using XOutput.Devices;

namespace XOutput.UI.Component
{
    /// <summary>
    /// Interaction logic for AxisView.xaml
    /// </summary>
    public partial class DPadView : UserControl, IUpdatableView, IViewBase<DPadViewModel, DPadModel>
    {
        public DPadViewModel ViewModel { get; private set; }

        public DPadView(DPadViewModel viewModel)
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
