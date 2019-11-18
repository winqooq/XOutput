using System.Windows.Controls;
using XOutput.Devices;

namespace XOutput.UI.Component
{
    /// <summary>
    /// Interaction logic for AxisView.xaml
    /// </summary>
    public partial class Axis2DView : UserControl, IUpdatableView, IViewBase<Axis2DViewModel, Axis2DModel>
    {
        public Axis2DViewModel ViewModel { get; private set; }

        public Axis2DView(Axis2DViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;
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
