/// Written by: Yulia Danilova
/// Creation Date: 24th of October, 2019
/// Purpose: Asynchronous non-generic and generic implementation of ICommand interface
#region ========================================================================= USING =====================================================================================
using System.Windows.Input;
using System.Threading.Tasks;
using System;
#endregion

namespace LiveChartMemoryLeak.MVVM
{
    public interface IAsyncCommand : ICommand, IDisposable
    {
        #region ================================================================ METHODS ====================================================================================
        /// <summary>
        /// Defines the asynchronous method to be called when the command is invoked.
        /// </summary>
        Task ExecuteAsync();

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state
        /// </summary>
        /// <returns>True if this command can be executed; otherwise, False.</returns>
        bool CanExecute();

        /// <summary>
        /// Executes the delegate that signals changes in permissions of execution of the command
        /// </summary>
        void RaiseCanExecuteChanged();
        #endregion
    }

    public interface IAsyncCommand<T> : ICommand, IDisposable
    {
        #region ================================================================ METHODS ====================================================================================
        /// <summary>
        /// Defines the generic asynchronous method to be called when the command is invoked.
        /// </summary>
        /// <param name="param">Generic parameter passed to the command</param>
        Task ExecuteAsync(T param);

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state
        /// </summary>
        /// <param name="param">Generic parameter passed to the command</param>
        /// <returns>True if this command can be executed; otherwise, False.</returns>
        bool CanExecute(T param);
        #endregion
    }   
}
