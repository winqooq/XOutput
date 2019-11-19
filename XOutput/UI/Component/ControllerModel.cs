using System.Windows.Media;
using XOutput.Devices;
using XOutput.Tools;

namespace XOutput.UI.Component
{
    public class ControllerModel : ModelBase
    {
        private GameController controller;
        public GameController Controller
        {
            get => controller;
            set { Set(value, ref controller, nameof(Controller)); }
        }

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
        public string DisplayName => Controller.ToString();

        public void RefreshName()
        {
            OnPropertyChanged(nameof(DisplayName));
        }

        [ResolverMethod]
        public ControllerModel()
        {

        }
    }
}
