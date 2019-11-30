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

        private bool isAxis;
        public bool IsAxis
        {
            get => isAxis;
            set { Set(value, ref isAxis, nameof(IsAxis)); }
        }

        private bool halfAxisMode;
        public bool HalfAxisMode
        {
            get => halfAxisMode;
            set { Set(value, ref halfAxisMode, nameof(HalfAxisMode)); }
        }

        private XInputTypes inputType;
        public XInputTypes InputType
        {
            get => inputType;
            set { Set(value, ref inputType, nameof(InputType)); }
        }


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
