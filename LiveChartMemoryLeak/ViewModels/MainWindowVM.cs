/// Written by: Yulia Danilova
/// Creation Date: 11th of November, 2020
/// Purpose: View Model for the main application Window
#region ========================================================================= USING =====================================================================================
using System;
using System.Linq;
using System.Collections.Generic;
#endregion

namespace LiveChartMemoryLeak
{
    public class MainWindowVM : IMainWindowVM
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        public event Action? SalesDataUpdated;

        private readonly IViewFactory viewFactory;
        #endregion

        #region ================================================================ PROPERTIES =================================================================================
        public Dictionary<string, IEnumerable<double>>? Sales { get; set; }
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// </summary>
        /// <param name="notificationService">Injected notification service</param>
        public MainWindowVM(IViewFactory viewFactory)
        {
            this.viewFactory = viewFactory;
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Pupulates the chart with the sales of past two years, grouped by month
        /// </summary>
        public void GetSalesChartDataAsync()
        {
            List<int> dates = new();
            Dictionary<int, decimal> values = new();
            Random random = new();
            int month = 0;
            for (int i = 0; i < 48; i++)
            {
                month++;
                values.Add(Convert.ToInt32(DateTime.Now.AddYears(-4).AddMonths(month).Date.ToString("yyMM")), random.Next(100, 1000));
                dates.Add(Convert.ToInt32(DateTime.Now.AddYears(-4).AddMonths(month).Date.ToString("yyMM")));
            }
            dates.Sort();
            // declare a list that will hold the years taken from database (only unique entries!)
            HashSet<string> years = new();
            // declare a list that will contain lists for each taken year, which lists will contain the sales
            List<List<double>> sales = new()
            {
                // initialize with first year list
                new List<double>()
            };
            int index = -1;
            // keep a record of the first displayed month in the chart
            int startingMonth = DateTime.Now.Month;
            // iterate the months
            foreach (int key in dates)
            {
                // if the years list does not contain current iterated year, increment the index so that it indicates that a new year sub-list must be added to the Sales
                if (!years.Contains(key.ToString()[..2]))
                    index++;
                // check if you need to add a new sub-list for a new year
                if (sales.Count == index)
                    sales.Add(new List<double>());
                // add the sales values for the current year
                sales[index].Add(Convert.ToDouble(values[key]));
                // add current year to years list (since its a HashSet, it won't allow duplicates)
                years.Add(key.ToString()[..2]);
            }
            // reset the index
            index = 0;
            // iterate through the unique years list
            foreach (string year in years)
            {
                // when index is 0 (the sales of first year), we need to initialize the dictionary. 
                // first year is always the first sub-list in Sales, which contains the months from current month to end of year
                if (index == 0)
                {
                    Sales = new Dictionary<string, IEnumerable<double>>()
                    {
                        { "20" + year, sales[index] }
                    };
                }
                // for last year, we need to take the months from start of year to current month
                else if (index == years.Count - 1)// 2)
                {
                    // in test production medium, the database may not contain data from current month, watch for exceptions!
                    try
                    {
                        Sales!.Add("20" + year, sales[index].Skip(startingMonth - 1)
                                                            .Take(startingMonth)
                                                            .Concat(new double[12 - startingMonth])
                                                            .Concat(sales[index]
                                                            .Take(startingMonth - 1)));
                    }
                    catch (Exception)
                    {
                        Sales!.Add("20" + year, sales[index]);
                    }
                }
                // for the middle years, since we start displaying from the current month, we need to split the array in two, 
                // and take first the months from current month to end of year, then from start of year to current month
                else
                {
                    Sales!.Add("20" + year, sales[index].Skip(startingMonth - 1)
                                                        .Take(12)
                                                        .Concat(sales[index]
                                                        .Take(startingMonth - 1)));
                }
                index++;
            }
            SalesDataUpdated?.Invoke();
        }
        #endregion
    }
}
