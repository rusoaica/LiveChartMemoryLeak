/// Written by: Yulia Danilova
/// Creation Date: 11th of November, 2020
/// Purpose: Interface for the view model for the main application view
#region ========================================================================= USING =====================================================================================
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
#endregion

namespace LiveChartMemoryLeak
{
    public interface IMainWindowVM 
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        event Action? SalesDataUpdated;
        #endregion

        #region ================================================================ PROPERTIES =================================================================================
        Dictionary<string, IEnumerable<double>>? Sales { get; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Pupulates the chart with the sales of past four years, grouped by month
        /// </summary>
        void GetSalesChartDataAsync();
        #endregion
    }
}
