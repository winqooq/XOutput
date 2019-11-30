using XOutput.Devices;
using XOutput.Tools;

namespace XOutput.UI.Component
{
    public class ButtonViewModel : ViewModelBase<ButtonModel>
    {
        [ResolverMethod(Scope.Prototype)]
        public ButtonViewModel(ButtonModel model) : base(model)
        {

        }

        public void Initialize(InputSource type)
        {
            Model.Type = type;
        }

        public void UpdateValues(IDevice device)
        {
            Model.Value = device.Get(Model.Type) > 0.5;
        }
    }
}
