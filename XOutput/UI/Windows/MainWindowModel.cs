using System.Collections.ObjectModel;
using XOutput.Tools;
using XOutput.UI.Component;

namespace XOutput.UI.Windows
{
    public class MainWindowModel : ModelBase
    {
        private readonly ObservableCollection<InputView> inputs = new ObservableCollection<InputView>();
        public ObservableCollection<InputView> Inputs { get { return inputs; } }

        private readonly ObservableCollection<ControllerView> controllers = new ObservableCollection<ControllerView>();
        public ObservableCollection<ControllerView> Controllers { get { return controllers; } }

        [ResolverMethod(Scope.Prototype)]
        public MainWindowModel()
        {

        }

        public override void CleanUp()
        {
            base.CleanUp();
            Inputs.Clear();
            Controllers.Clear();
        }
    }
}
