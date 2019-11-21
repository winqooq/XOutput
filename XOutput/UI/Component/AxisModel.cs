using XOutput.Devices;
using XOutput.Tools;

namespace XOutput.UI.Component
{
    public class AxisModel : ModelBase
    {
        private InputSource type;
        public InputSource Type
        {
            get => type;
            set { Set(value, ref type, nameof(Type)); }
        }
        private int value;
        public int Value
        {
            get => value;
            set { Set(value, ref this.value, nameof(Value)); }
        }
        private int max;
        public int Max
        {
            get => max;
            set { Set(value, ref max, nameof(Max)); }
        }

        [ResolverMethod(Scope.Prototype)]
        public AxisModel()
        {

        }
    }
}
