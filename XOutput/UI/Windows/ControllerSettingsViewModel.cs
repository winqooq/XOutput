using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;
using XOutput.Devices;
using XOutput.Devices.Input;
using XOutput.Devices.Input.DirectInput;
using XOutput.Devices.XInput;
using XOutput.Tools;
using XOutput.UI.Component;

namespace XOutput.UI.Windows
{
    public class ControllerSettingsViewModel : ViewModelBase<ControllerSettingsModel>
    {
        private GameController controller;

        [ResolverMethod(Scope.Prototype)]
        public ControllerSettingsViewModel(ControllerSettingsModel model) : base(model)
        {

        }

        public void Initialize(GameController gameController)
        {
            controller = gameController;
            Model.Title = controller.DisplayName;
            CreateMappingControls();
            CreateXInputControls();
            Model.StartWhenConnected = controller.Mapper.StartWhenConnected;
            Model.ForceFeedbacks.Add(new ComboBoxItem { Content = new TextBlock(new Run("")) });
            var devices = InputDevices.Instance.GetDevices().OfType<DirectDevice>().ToArray();
            foreach (var device in devices)
            {
                var item = new ComboBoxItem
                {
                    Tag = device,
                    Content = new TextBlock(new Run(device.DisplayName)),
                };
                Model.ForceFeedbacks.Add(item);
                if (!string.IsNullOrEmpty(controller.Mapper.ForceFeedbackDevice) && controller.Mapper.ForceFeedbackDevice == device.UniqueId)
                {
                    Model.ForceFeedback = item;
                }
            }
        }

        public void ConfigureAll()
        {
            var types = XInputHelper.Instance.Values;
            new AutoConfigureWindow(new AutoConfigureViewModel(new AutoConfigureModel(), InputDevices.Instance.GetDevices(), controller.Mapper, types.ToArray()), types.Any()).ShowDialog();
            // foreach (var v in Model.MapperAxisViews.Concat(Model.MapperButtonViews).Concat(Model.MapperDPadViews))
            foreach (var v in Model.MapperAxisViews.Concat(Model.MapperDPadViews))
            {
                v.Refresh();
            }
        }

        public void Update()
        {
            UpdateXInputControls();
        }

        public void SetForceFeedback()
        {
            if (Model.ForceFeedback.Tag != null)
            {
                var device = Model.ForceFeedback.Tag as IInputDevice;
                controller.Mapper.ForceFeedbackDevice = device?.UniqueId;
                controller.ForceFeedbackDevice = device;
            }
            else
            {
                controller.Mapper.ForceFeedbackDevice = null;
                controller.ForceFeedbackDevice = null;
            }
        }

        public void SetStartWhenConnected()
        {
            controller.Mapper.StartWhenConnected = Model.StartWhenConnected;
        }

        private void CreateMappingControls()
        {
            foreach (var xInputType in XInputHelper.Instance.Buttons)
            {
                var view = ApplicationContext.Global.Resolve<MappingBlockView>();
                view.Initialize(new MappingBlockContext { Controller = controller, InputType = xInputType });
                Model.MapperButtonViews.Add(view);

            }
            foreach (var xInputType in XInputHelper.Instance.DPad)
            {
                var view = ApplicationContext.Global.Resolve<MappingView>();
                view.Initialize(new MappingContext { Controller = controller, InputType = xInputType });
                Model.MapperDPadViews.Add(view);
            }
            foreach (var xInputType in XInputHelper.Instance.Axes)
            {
                var view = ApplicationContext.Global.Resolve<MappingView>();
                view.Initialize(new MappingContext { Controller = controller, InputType = xInputType });
                Model.MapperAxisViews.Add(view);
            }
            foreach (var xInputType in XInputHelper.Instance.Sliders)
            {
                var view = ApplicationContext.Global.Resolve<MappingView>();
                view.Initialize(new MappingContext { Controller = controller, InputType = xInputType });
                Model.MapperAxisViews.Add(view);
            }
        }

        private void CreateXInputControls()
        {
            foreach (var buttonInput in controller.XInput.Sources.Where(s => s.Type == InputSourceTypes.Button))
            {
                var buttonView = ApplicationContext.Global.Resolve<ButtonView>();
                buttonView.Initialize(new ButtonContext { Source = buttonInput });
                Model.XInputButtonViews.Add(buttonView);
            }
            var dPadView = ApplicationContext.Global.Resolve<DPadView>();
            dPadView.Initialize(new DPadContext { Device = controller.XInput, DPadIndex = 0, ShowLabel = false });
            Model.XInputDPadViews.Add(dPadView);
            var lx = controller.XInput.Sources.OfType<XOutputSource>().First(s => s.XInputType == XInputTypes.LX);
            var ly = controller.XInput.Sources.OfType<XOutputSource>().First(s => s.XInputType == XInputTypes.LY);
            var rx = controller.XInput.Sources.OfType<XOutputSource>().First(s => s.XInputType == XInputTypes.RX);
            var ry = controller.XInput.Sources.OfType<XOutputSource>().First(s => s.XInputType == XInputTypes.RY);
            var l2 = controller.XInput.Sources.OfType<XOutputSource>().First(s => s.XInputType == XInputTypes.L2);
            var r2 = controller.XInput.Sources.OfType<XOutputSource>().First(s => s.XInputType == XInputTypes.R2);
            var lAxes = ApplicationContext.Global.Resolve<Axis2DView>().InitializeWith(v => { v.ViewModel.Initialize(lx, ly); });
            Model.XInputAxisViews.Add(lAxes);
            var rAxes = ApplicationContext.Global.Resolve<Axis2DView>();
            rAxes.ViewModel.Initialize(rx, ry);
            Model.XInputAxisViews.Add(rAxes);
            var l2View = ApplicationContext.Global.Resolve<AxisView>();
            l2View.ViewModel.Initialize(l2);
            Model.XInputAxisViews.Add(l2View);
            var r2View = ApplicationContext.Global.Resolve<AxisView>();
            r2View.ViewModel.Initialize(l2);
            Model.XInputAxisViews.Add(r2View);
        }

        private void UpdateXInputControls()
        {
            foreach (var axisView in Model.XInputAxisViews)
            {
                axisView.UpdateValues();
            }
            foreach (var buttonView in Model.XInputButtonViews)
            {
                buttonView.UpdateValues();
            }
            foreach (var dPadView in Model.XInputDPadViews)
            {
                dPadView.UpdateValues();
            }
        }
    }
}
