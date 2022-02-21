/// Written by: Yulia Danilova
/// Creation Date: 11th of June, 2021
/// Purpose: Registers services in the Autofac Dependency Injection container
#region ========================================================================= USING =====================================================================================
using Autofac;
#endregion

namespace LiveChartMemoryLeak
{
    internal class DIContainerConfig
    {
        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Registers and configures the services used by the application in the Autofac Dependency Injection container
        /// </summary>
        /// <returns>The container that creates, wires dependencies and manages lifetimes of the registered services</returns>
        public static IContainer Configure()
        {
            ContainerBuilder builder = new();

            // register view models
            builder.RegisterType<StartupVM>().As<IStartupVM>().InstancePerDependency();
            builder.RegisterType<MainWindowVM>().As<IMainWindowVM>().InstancePerDependency();  
            
            // register views and assign them the above view models, upon activation
            builder.RegisterType<Startup>()
                   .As<IStartupView>()
                   .OnActivating(e => e.Instance!.DataContext = e.Context.Resolve<IStartupVM>())
                   .InstancePerDependency();
            builder.RegisterType<MainWindow>()
                   .As<IMainWindowView>()
                   .OnActivating(e => e.Instance!.DataContext = e.Context.Resolve<IMainWindowVM>())
                   .InstancePerDependency(); 

            // register the abstract factory for creating views
            builder.RegisterType<ViewFactory>().As<IViewFactory>().SingleInstance();
          
            return builder.Build();
        }
        #endregion
    }
}
