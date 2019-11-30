using XOutput.Devices;
using XOutput.Tools;

namespace XOutput.UI.Component
{
    public class ButtonModel : ModelBase
    {
        private InputSource type;
        public InputSource Type
        {
            get => type;
            set { Set(value, ref type, nameof(Type)); }
        }
        private bool value;
        public bool Value
        {
            get => value;
            set { Set(value, ref this.value, nameof(Value)); }
        }

        [ResolverMethod(Scope.Prototype)]
        public ButtonModel()
        {

        }
    }
}
