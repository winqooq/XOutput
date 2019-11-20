using System.Windows.Media;
using XOutput.Devices;
using XOutput.Tools;

namespace XOutput.UI.Component
{
    public class ControllerModel : ModelBase
    {
        private string buttonText;
        public string ButtonText
        {
            get => buttonText;
            set { Set(value, ref buttonText, nameof(ButtonText)); }
        }
        private bool started;
        public bool Started
        {
            get => started;
            set { Set(value, ref started, nameof(Started)); }
        }

        private bool canStart;
        public bool CanStart
        {
            get => canStart;
            set { Set(value, ref canStart, nameof(CanStart)); }
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
        public ControllerModel()
        {

        }
    }
}
