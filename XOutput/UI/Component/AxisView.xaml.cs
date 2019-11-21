using System.Windows.Controls;
using XOutput.Devices;
using XOutput.Tools;

namespace XOutput.UI.Component
{
    public partial class AxisView : ComponentBase<AxisViewModel, AxisModel, AxisContext>, IUpdatableView
    {
        [ResolverMethod(Scope.Prototype)]
        public AxisView(AxisViewModel viewModel) : base(viewModel)
        {
            InitializeComponent();
        }

        public override void Initialize(AxisContext context)
        {
            ViewModel.Initialize(context.Source);
        }

        public void UpdateValues(IDevice device)
        {
            ViewModel.UpdateValues(device);
        }
    }
}
