/// Written by: Yulia Danilova
/// Creation Date: 12th of November, 2020
/// Purpose: View Model for the startup view
#region ========================================================================= USING =====================================================================================
using System;
using System.Diagnostics;
using LiveChartMemoryLeak.MVVM;
#endregion

namespace LiveChartMemoryLeak
{
    public class StartupVM : IStartupVM
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private readonly IViewFactory viewFactory;
        #endregion

        #region ============================================================= BINDING COMMANDS ==============================================================================
        public ISyncCommand OpenMainWindow_Command { get; private set; }
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// <param name="viewFactory">The injected abstract factory for creating views</param>
        /// </summary>
        public StartupVM(IViewFactory viewFactory)
        {
            this.viewFactory = viewFactory;
            OpenMainWindow_Command = new SyncCommand(OpenMainWindow);
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================  
        /// <summary>
        /// Opens a new main application view
        /// </summary>
        private async void OpenMainWindow()
        {
            IMainWindowView? view = viewFactory.CreateView<IMainWindowView>();
            view.ShowDialog();
            await System.Threading.Tasks.Task.Delay(3000);

            // force memory collection, and call it twice:
            // https://docs.microsoft.com/en-us/answers/questions/350963/why-gccollect-call-twice-required-when-use-gcwaitf.html
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            Trace.WriteLine("collected");
        }

        public void Dispose()
        {
            
        }
        #endregion

    }
}
