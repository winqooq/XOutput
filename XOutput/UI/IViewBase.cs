namespace XOutput.UI
{
    public interface IViewBase<VM, M> : ICleanUp where VM : ViewModelBase<M> where M : ModelBase
    {
        VM ViewModel { get; }
    }
}
