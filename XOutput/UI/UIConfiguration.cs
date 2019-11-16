using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using XOutput.Tools;
using XOutput.UI.Windows;

namespace XOutput.UI
{
    public static class UIConfiguration
    {
        [ResolverMethod(Scope.Prototype)]
        public static DiagnosticsViewModel GetDiagnosticsViewModel(DiagnosticsModel model)
        {
            return new DiagnosticsViewModel(model);
        }

        [ResolverMethod(Scope.Prototype)]
        public static DiagnosticsWindow GetDiagnosticsWindow(DiagnosticsViewModel viewModel)
        {
            return new DiagnosticsWindow(viewModel);
        }
    }
}
