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

        public void UpdateValues()
        {
            Model.Value = Model.Type.Value > 0.5;
        }
    }
}
