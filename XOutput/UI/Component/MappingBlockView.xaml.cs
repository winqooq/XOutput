using System.Windows;
using System.Windows.Controls;
using XOutput.Tools;

namespace XOutput.UI.Component
{
    public partial class MappingBlockView : ComponentBase<MappingBlockViewModel, MappingBlockModel, MappingBlockContext>
    {

        [ResolverMethod(Scope.Prototype)]
        public MappingBlockView(MappingBlockViewModel viewModel) : base(viewModel)
        {
            InitializeComponent();
        }

        public override void Initialize(MappingBlockContext context)
        {

        }
    }
}
