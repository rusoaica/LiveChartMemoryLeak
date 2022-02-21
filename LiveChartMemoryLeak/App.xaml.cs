/// Written by: Yulia Danilova
/// Creation Date: 08th of December, 2021
/// Purpose: Application's entry point class
#region ========================================================================= USING =====================================================================================
using Autofac;
using System.Windows;
#endregion

namespace LiveChartMemoryLeak
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Framework initialization code
        /// </summary>
        private void OnApplicationStartup(object sender, StartupEventArgs e)
        {
            // configure the dependency injection services
            IContainer container = DIContainerConfig.Configure();
            // begin the application's lifetime score
            using (ILifetimeScope scope = container.BeginLifetimeScope())
            {
                // get a view factory from the DI container and display the startup view from it, as modal dialog
                IViewFactory viewFactory = container.Resolve<IViewFactory>();
                IStartupView startupView = viewFactory.CreateView<IStartupView>();
                startupView.Show();
            }
        }
        #endregion
    }
}
