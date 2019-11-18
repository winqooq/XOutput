using System.ComponentModel;

namespace XOutput.UI
{
    public abstract class ViewModelBase<M> where M : ModelBase, INotifyPropertyChanged
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
}
