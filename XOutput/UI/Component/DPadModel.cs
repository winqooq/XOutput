using XOutput.Devices;
using XOutput.Tools;

namespace XOutput.UI.Component
{
    public class DPadModel : ModelBase
    {
        private DPadDirection direction;
        public DPadDirection Direction
        {
            get => direction;
            set { Set(value, ref direction, nameof(Direction)); }
        }

        private int valuex;
        public int ValueX
        {
            get => valuex;
            set { Set(value, ref valuex, nameof(ValueX)); }
        }

        private int valuey;
        public int ValueY
        {
            get => valuey;
            set { Set(value, ref valuey, nameof(ValueY)); }
        }

        private string label;
        public string Label
        {
            get => label;
            set { Set(value, ref label, nameof(Label)); }
        }

        [ResolverMethod(Scope.Prototype)]
        public DPadModel()
        {

        }
    }
}
