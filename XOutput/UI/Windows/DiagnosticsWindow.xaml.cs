using System;
using System.Windows;

namespace XOutput.UI.Windows
{
    /// <summary>
    /// Interaction logic for AutoConfigureWindow.xaml
    /// </summary>
    public partial class DiagnosticsWindow : Window, IViewBase<DiagnosticsViewModel, DiagnosticsModel>
    {
        public DiagnosticsViewModel ViewModel { get; private set; }

        public DiagnosticsWindow(DiagnosticsViewModel viewModel)
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
