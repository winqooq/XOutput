using System.Windows.Controls;
using XOutput.Devices;

namespace XOutput.UI.Component
{
    /// <summary>
    /// Interaction logic for ButtonView.xaml
    /// </summary>
    public partial class ButtonView : UserControl, IUpdatableView, IViewBase<ButtonViewModel, ButtonModel>
    {
        public ButtonViewModel ViewModel { get; private set; }

        public ButtonView(ButtonViewModel viewModel)
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
