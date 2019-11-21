using System.Windows;
using System.Windows.Controls;
using XOutput.Tools;

namespace XOutput.UI.Component
{
    public partial class MappingBlockView : UserControl, IViewBase<MappingViewModel, MappingModel>
    {
        public MappingViewModel ViewModel { get; private set; }

        [ResolverMethod(Scope.Prototype)]
        public MappingBlockView(MappingViewModel viewModel)
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
