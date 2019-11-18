using System.Windows;
using System.Windows.Controls;
using XOutput.Devices.XInput;

namespace XOutput.UI.Component
{
    public partial class XBox : Viewbox, IViewBase<XBoxViewModel, XBoxModel>
    {
        public static readonly DependencyProperty XInputTypeProperty = DependencyProperty.Register("XInputType", typeof(XInputTypes), typeof(XBox), new FrameworkPropertyMetadata(OnXInputTypeChanged, null));
        public static readonly DependencyProperty HighlightProperty = DependencyProperty.Register("Highlight", typeof(bool), typeof(XBox), new FrameworkPropertyMetadata(OnHightlightChanged, null));

        private static void OnXInputTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var xbox = (XBox)d;
            xbox.ViewModel.Model.XInputType = (XInputTypes)e.NewValue;
        }

        private static void OnHightlightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var xbox = (XBox)d;
            xbox.ViewModel.Model.Highlight = (bool)e.NewValue;
        }

        public XInputTypes XInputType
        {
            get { return (XInputTypes)GetValue(XInputTypeProperty); }
            set { SetValue(XInputTypeProperty, value); ViewModel.Model.XInputType = value; }
        }
        public bool Highlight
        {
            get { return (bool)GetValue(HighlightProperty); }
            set { SetValue(HighlightProperty, value); ViewModel.Model.Highlight = value; }
        }
        public XBoxViewModel ViewModel { get; private set; }

        public XBox()
        {
            ViewModel = new XBoxViewModel(new XBoxModel());
            DataContext = ViewModel;
            InitializeComponent();
        }

        public void CleanUp()
        {
            ViewModel.CleanUp();
        }
    }
}
