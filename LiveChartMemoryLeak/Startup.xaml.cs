/// Written by: Yulia Danilova
/// Creation Date: 24th of November, 2019
/// Purpose: Code behind for the StartupV view
#region ========================================================================= USING =====================================================================================
using System;
using System.Windows;
#endregion

namespace LiveChartMemoryLeak
{
    /// <summary>
    /// Interaction logic for Startup.xaml
    /// </summary>
    public partial class Startup : Window, IStartupView
    {
        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Default C-tor
        /// </summary>
        public Startup()
        {
            InitializeComponent();
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            DataContext = null;
        }
        #endregion
    }
}
