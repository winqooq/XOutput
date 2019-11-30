using System;
using XOutput.Devices;
using XOutput.Devices.Input;
using XOutput.Devices.Mapper;
using XOutput.Devices.XInput;
using XOutput.Tools;
using XOutput.UI.Windows;

namespace XOutput.UI.Component
{
    public class MappingBlockViewModel : ViewModelBase<MappingBlockModel>
    {
        private GameController controller;
        private XInputTypes inputType;

        [ResolverMethod(Scope.Prototype)]
        public MappingBlockViewModel(MappingBlockModel model) : base(model)
        {

        }

        internal void Initialize(GameController controller, XInputTypes inputType)
        {
            this.controller = controller;
            this.inputType = inputType;
            Model.IsAxis = inputType.IsAxis();
            Model.InputType = inputType;
            Model.HalfAxisMode = false; // TODO
        }

        internal void AddInput()
        {
            var view = ApplicationContext.Global.Resolve<MappingView>();
            view.Initialize(new MappingContext { Controller = controller, InputType = inputType });
            Model.MappingViews.Add(view);
        }
    }
}
