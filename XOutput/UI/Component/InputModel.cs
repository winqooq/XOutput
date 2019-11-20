using System.Windows.Media;
using XOutput.Devices.Input;
using XOutput.Tools;

namespace XOutput.UI.Component
{
    public class InputModel : ModelBase
    {
        private IInputDevice device;
        public IInputDevice Device
        {
            get => device;
            set { Set(value, ref device, nameof(Device)); }
        }

        private Brush background;
        public Brush Background
        {
            get => background;
            set { Set(value, ref background, nameof(Background)); }
        }

        private string displayName;
        public string DisplayName
        {
            get => displayName;
            set { Set(value, ref displayName, nameof(DisplayName)); }
        }

        [ResolverMethod(Scope.Prototype)]
        public InputModel()
        {

        }
    }
}
