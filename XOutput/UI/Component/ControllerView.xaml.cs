using System;
using System.Windows;
using System.Windows.Controls;
using XOutput.Devices;
using XOutput.Tools;

namespace XOutput.UI.Component
{
    /// <summary>
    /// Interaction logic for ControllerView.xaml
    /// </summary>
    public partial class ControllerView : UserControl, IViewBase<ControllerViewModel, ControllerModel>
    {
        public event Action<ControllerView, GameController> RemoveClicked;
        public ControllerViewModel ViewModel { get; private set; }

        private GameController controller;

        [ResolverMethod(Scope.Prototype)]
        public ControllerView(ControllerViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = viewModel;
            InitializeComponent();
        }

        public void Initialize(ControllerContext context)
        {
            controller = context.Controller;
            ViewModel.Initialize(context.Controller);
        }

        public void CleanUp()
        {
            ViewModel.CleanUp();
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
            RemoveClicked?.Invoke(this, this.controller);
        }
    }
}
