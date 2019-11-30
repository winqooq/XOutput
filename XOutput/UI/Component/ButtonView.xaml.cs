using System.Windows.Controls;
using XOutput.Devices;
using XOutput.Tools;

namespace XOutput.UI.Component
{
    public partial class ButtonView : ComponentBase<ButtonViewModel, ButtonModel, ButtonContext>, IUpdatableView
    {
        [ResolverMethod(Scope.Prototype)]
        public ButtonView(ButtonViewModel viewModel) : base(viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }

        public override void Initialize(ButtonContext context)
        {
            ViewModel.Initialize(context.Source);
        }

        public void UpdateValues(IDevice device)
        {
            ViewModel.UpdateValues(device);
        }
    }
}
