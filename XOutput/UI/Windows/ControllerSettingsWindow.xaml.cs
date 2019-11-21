using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using XOutput.Devices;
using XOutput.Tools;

namespace XOutput.UI.Windows
{
    public partial class ControllerSettingsWindow : WindowBase<ControllerSettingsViewModel, ControllerSettingsModel, ControllerSettingsContext>
    {
        private readonly DispatcherTimer timer = new DispatcherTimer();
        private GameController controller;

        [ResolverMethod(Scope.Prototype)]
        public ControllerSettingsWindow(ControllerSettingsViewModel viewModel) : base(viewModel)
        {
            InitializeComponent();
        }

        public override void Initialize(ControllerSettingsContext context)
        {
            controller = context.Controller;
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += TimerTick;
            ViewModel.Initialize(controller);
            ViewModel.Update();
            timer.Start();
        }

        public override void CleanUp()
        {
            timer.Tick -= TimerTick;
            timer.Stop();
            ViewModel.CleanUp();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            ViewModel.Update();
        }

        private void ConfigureAllButtonClick(object sender, RoutedEventArgs e)
        {
            ViewModel.ConfigureAll();
        }

        private void CheckBoxChecked(object sender, RoutedEventArgs e)
        {
            ViewModel.SetStartWhenConnected();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.SetForceFeedback();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            controller.Mapper.Name = ViewModel.Model.Title;
        }
    }
}
