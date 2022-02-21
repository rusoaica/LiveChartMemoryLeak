/// Written by: Yulia Danilova
/// Creation Date: 11th of June, 2021
/// Purpose: Factory for creating views
#region ========================================================================= USING =====================================================================================
using System;
using Autofac;
using System.Linq;
using Autofac.Features.OwnedInstances;
#endregion

namespace LiveChartMemoryLeak
{
    internal class ViewFactory : IViewFactory
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private readonly ILifetimeScope scope;
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// </summary>
        /// <param name="scope">The injected scope to be used</param>
        public ViewFactory(ILifetimeScope scope)
        {
            this.scope = scope;
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Creates a view of type <typeparamref name="TResult"/>
        /// </summary>
        /// <typeparam name="TResult">The type of view to create</typeparam>
        /// <returns>A view of type <typeparamref name="TResult"/></returns>
        public TResult CreateView<TResult>() where TResult : IView
        {
            Owned<TResult>? view = scope.Resolve<Owned<TResult>>();
            view.Value.Closing += (s, e) => { view.Dispose(); };
            return view.Value;
        }
        #endregion
    }
}
