using System.Linq;
using XOutput.Devices;
using XOutput.Tools;

namespace XOutput.UI.Component
{
    public class DPadViewModel : ViewModelBase<DPadModel>
    {
        private const int len = 21;
        private int dPadIndex;
        private IDevice device;

        [ResolverMethod(Scope.Prototype)]
        public DPadViewModel(DPadModel model) : base(model)
        {

        }

        public void Initialize(IDevice device, int dPadIndex, bool showLabel)
        {
            this.device = device;
            this.dPadIndex = dPadIndex;
            if (showLabel)
            {
                Model.Label = "DPad" + (dPadIndex + 1);
            }
        }

        public void UpdateValues()
        {
            Model.Direction = device.DPads.ElementAt(dPadIndex);
            if (Model.Direction.HasFlag(DPadDirection.Up))
            {
                Model.ValueY = -len;
            }
            else if (Model.Direction.HasFlag(DPadDirection.Down))
            {
                Model.ValueY = len;
            }
            else
            {
                Model.ValueY = 0;
            }
            Model.ValueY += 21;
            if (Model.Direction.HasFlag(DPadDirection.Right))
            {
                Model.ValueX = len;
            }
            else if (Model.Direction.HasFlag(DPadDirection.Left))
            {
                Model.ValueX = -len;
            }
            else
            {
                Model.ValueX = 0;
            }
            Model.ValueX += 21;
        }
    }
}
