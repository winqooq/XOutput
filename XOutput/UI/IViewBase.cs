using System;

namespace XOutput.UI
{
    public interface IViewBase : ICleanUp
    {

    }

    public interface IInitializableViewBase<C> : IViewBase, IInitializable<C>
    {

    }

    public interface IViewBase<VM, M> : IViewBase where VM : ViewModelBase<M> where M : ModelBase
    {
        VM ViewModel { get; }
    }

    public interface IInitializableViewBase<VM, M, C> : IViewBase<VM, M>, IInitializableViewBase<C> where VM : ViewModelBase<M> where M : ModelBase
    {

    }

    public static class ViewInitializer
    {
        public static V InitializeWith<V, C>(this V view, C context) where V : IInitializableViewBase<C>
        {
            view.Initialize(context);
            return view;
        }

        public static V InitializeWith<V>(this V view, Action<V> initializer) where V : IViewBase
        {
            initializer(view);
            return view;
        }
    }
}
