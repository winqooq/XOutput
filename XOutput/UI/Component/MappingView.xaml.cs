using System.Windows;
using System.Windows.Controls;
using XOutput.Tools;

namespace XOutput.UI.Component
{
    /// <summary>
    /// Interaction logic for MappingView.xaml
    /// </summary>
    public partial class MappingView : ComponentBase<MappingViewModel, MappingModel, MappingContext>
    {
        [ResolverMethod(Scope.Prototype)]
        public MappingView(MappingViewModel viewModel)  : base(viewModel)
        {
            InitializeComponent();
        }

        public override void Initialize(MappingContext context)
        {
            ViewModel.Initialize(context.Controller, context.InputType);
        }

        public void Refresh()
        {
            ViewModel.Refresh();
        }

        private void ConfigureClick(object sender, RoutedEventArgs e)
        {
            ViewModel.Configure();
            Refresh();
        }

        private void InvertClick(object sender, RoutedEventArgs e)
        {
            ViewModel.Invert();
        }
    }
}
