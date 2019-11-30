using System.Collections.ObjectModel;
using System.Windows;
using XOutput.Devices;
using XOutput.Devices.Mapper;
using XOutput.Devices.XInput;
using XOutput.Tools;

namespace XOutput.UI.Component
{
    public class MappingBlockModel : ModelBase
    {
        public ObservableCollection<MappingView> MappingViews { get; private set; }
        public ObservableCollection<IUpdatableView> XInputViews { get; private set; }

        [ResolverMethod(Scope.Prototype)]
        public MappingBlockModel()
        {
            MappingViews = new ObservableCollection<MappingView>();
            XInputViews = new ObservableCollection<IUpdatableView>();
        }

        public override void CleanUp()
        {
            base.CleanUp();
            MappingViews.ClearView();
            XInputViews.ClearView();
        }
    }
}
