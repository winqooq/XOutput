using System.Windows.Controls;
using XOutput.Devices;
using XOutput.Tools;

namespace XOutput.UI.Component
{
    public partial class Axis2DView : ComponentBase<Axis2DViewModel, Axis2DModel, Axis2DContext>, IUpdatableView
    {

        [ResolverMethod(Scope.Prototype)]
        public Axis2DView(Axis2DViewModel viewModel) : base(viewModel)
        {
            InitializeComponent();
        }

        public override void Initialize(Axis2DContext context)
        {
            ViewModel.Initialize(context.SourceX, context.SourceY);
        }

        public void UpdateValues(IDevice device)
        {
            ViewModel.UpdateValues(device);
        }
    }
}
