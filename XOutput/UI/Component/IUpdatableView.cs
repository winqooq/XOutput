using XOutput.Devices;

namespace XOutput.UI.Component
{
    public interface IUpdatableView : IViewBase
    {
        /// <summary>
        /// Updates the view.
        /// </summary>
        void UpdateValues();
    }
}
