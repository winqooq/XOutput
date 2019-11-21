using XOutput.Devices;
using XOutput.Tools;

namespace XOutput.UI.Component
{
    public class Axis2DModel : ModelBase
    {
        private InputSource typeX;
        public InputSource TypeX
        {
            get => typeX;
            set { Set(value, ref typeX, nameof(TypeX)); }
        }
        private InputSource typeY;
        public InputSource TypeY
        {
            get => typeY;
            set { Set(value, ref typeY, nameof(TypeY)); }
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

        private int maxx;
        public int MaxX
        {
            get => maxx;
            set { Set(value, ref maxx, nameof(MaxX)); }
        }

        private int maxy;
        public int MaxY
        {
            get => maxy;
            set { Set(value, ref maxy, nameof(MaxY)); }
        }

        [ResolverMethod(Scope.Prototype)]
        public Axis2DModel()
        {

        }
    }
}
