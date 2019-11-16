using System.ComponentModel;

namespace XOutput.UI
{
    public abstract class ResultViewModelBase<M, R> : ViewModelBase<M> where M : ModelBase, INotifyPropertyChanged
    {
        public R Result { get; set; }

        protected ResultViewModelBase(M model) : base(model)
        {

        }

        protected ResultViewModelBase(M model, R result) : base(model)
        {
            Result = result;
        }
    }
}
