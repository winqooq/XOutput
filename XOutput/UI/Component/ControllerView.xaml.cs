using System;
using System.Windows;
using System.Windows.Controls;
using XOutput.Devices;
using XOutput.Tools;

namespace XOutput.UI.Component
{
    public partial class ControllerView : ComponentBase<ControllerViewModel, ControllerModel, ControllerContext>
    {
        public event Action<ControllerView, GameController> RemoveClicked;

        private GameController controller;

        [ResolverMethod(Scope.Prototype)]
        public ControllerView(ControllerViewModel viewModel) : base(viewModel)
        {
            InitializeComponent();
        }

        public override void Initialize(ControllerContext context)
        {
            controller = context.Controller;
            ViewModel.Initialize(context.Controller);
        }

        private void OpenClick(object sender, RoutedEventArgs e)
        {
            ViewModel.Edit();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            ViewModel.StartStop();
        }

        private void RemoveClick(object sender, RoutedEventArgs e)
        {
            RemoveClicked?.Invoke(this, controller);
        }
    }
}
