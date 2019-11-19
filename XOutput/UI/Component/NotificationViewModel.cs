using XOutput.Tools;

namespace XOutput.UI.Component
{
    public class NotificationViewModel : ViewModelBase<NotificationModel>
    {
        [ResolverMethod(Scope.Prototype)]
        public NotificationViewModel(NotificationModel model) : base(model)
        {

        }
    }
}
