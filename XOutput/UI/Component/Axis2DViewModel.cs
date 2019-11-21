using XOutput.Devices;
using XOutput.Tools;

namespace XOutput.UI.Component
{
    public class Axis2DViewModel : ViewModelBase<Axis2DModel>
    {
        private const int maxx = 42;
        private const int maxy = 42;
        [ResolverMethod(Scope.Prototype)]
        public Axis2DViewModel(Axis2DModel model) : base(model)
        {
            Model.MaxX = maxx;
            Model.MaxY = maxy;
        }

        public void Initialize(InputSource typex, InputSource typey)
        {
            Model.TypeX = typex;
            Model.TypeY = typey;
        }

        public void UpdateValues(IDevice device)
        {
            Model.ValueX = (int)(device.Get(Model.TypeX) * Model.MaxX);
            Model.ValueY = (int)(Model.MaxY - device.Get(Model.TypeY) * Model.MaxY);
        }
    }
}
