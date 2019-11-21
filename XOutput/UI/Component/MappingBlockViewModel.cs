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
        private readonly GameController controller;

        [ResolverMethod(Scope.Prototype)]
        public MappingBlockViewModel(MappingBlockModel model) : base(model)
        {

        }
    }
}
