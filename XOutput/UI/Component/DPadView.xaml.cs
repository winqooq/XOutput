using System.Windows.Controls;
using XOutput.Devices;
using XOutput.Tools;

namespace XOutput.UI.Component
{
    /// <summary>
    /// Interaction logic for AxisView.xaml
    /// </summary>
    public partial class DPadView : ComponentBase<DPadViewModel, DPadModel, DPadContext>, IUpdatableView
    { 
        [ResolverMethod(Scope.Prototype)]
        public DPadView(DPadViewModel viewModel) : base(viewModel)
        {
            InitializeComponent();
        }

        public override void Initialize(DPadContext context)
        {
            ViewModel.Initialize(context.Device, context.DPadIndex, context.ShowLabel);
        }

        public void UpdateValues()
        {
            ViewModel.UpdateValues();
        }
    }
}
