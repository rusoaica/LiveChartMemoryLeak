/// Written by: Yulia Danilova
/// Creation Date: 11th of June, 2021
/// Purpose: Abstract factory for creating views

namespace LiveChartMemoryLeak
{
    public interface IViewFactory
    {
        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Creates a view of type <typeparamref name="TResult"/>
        /// </summary>
        /// <typeparam name="TResult">The type of view to create</typeparam>
        /// <returns>A view of type <typeparamref name="TResult"/></returns>
        TResult CreateView<TResult>() where TResult : IView;
        #endregion
    }
}
