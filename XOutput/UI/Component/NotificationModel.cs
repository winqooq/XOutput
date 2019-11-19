using XOutput.Devices.XInput;
using XOutput.Tools;

namespace XOutput.UI
{
    public class NotificationModel : ModelBase
    {
        private string text = "";
        public string Text
        {
            get => text;
            set { Set(value, ref text, nameof(Text)); }
        }

        private string[] textArguments = new string[0];
        public string[] TextArguments
        {
            get => textArguments;
            set { Set(value, ref textArguments, nameof(TextArguments)); }
        }

        private bool important;
        public bool Important
        {
            get => important;
            set { Set(value, ref important, nameof(Important)); }
        }

        [ResolverMethod(Scope.Prototype)]
        public NotificationModel()
        {

        }
    }
}
