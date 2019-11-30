using XOutput.Devices;
using XOutput.Tools;

namespace XOutput.UI.Component
{
    public class DPadContext
    {
        public IDevice Device { get; set; }
        public int DPadIndex { get; set; }
        public bool ShowLabel { get; set; }
    }
}
