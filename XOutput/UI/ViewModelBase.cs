using System.ComponentModel;

namespace XOutput.UI
{
    public abstract class ViewModelBase<M> : ICleanUp where M : ModelBase
    {
        public LanguageModel LanguageModel => LanguageModel.Instance;
        public M Model { get; private set; }

        protected ViewModelBase(M model)
        {
            Model = model;
        }

        public virtual void CleanUp()
        {
            Model.CleanUp();
        }
    }

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
