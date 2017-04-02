using System;
using System.Threading;
using System.Windows.Threading;

namespace ViewModelHelper
{
    /// <summary>
    /// Class: UIThread
    /// </summary>
    /// <remarks>Allows threaded code to switch over to the the UI thread</remarks>
    /// <example>
    /// UIThread UIThread = new UIThread();
    /// UIThread.Put(() =>
    /// {
    ///     Console.WriteLine("This is on the UI thread");
    /// });
    /// </example>
    public class UIThread
    {
        #region Private member variables and public properties

        private SynchronizationContext _creationSyncContext;
        private readonly Thread _creationThread;

        /// <summary>
        /// Gets or sets the dispatcher.
        /// </summary>
        /// <value>The dispatcher.</value>
        /// <remarks>This is only used for unit testing. It is required because unit tests do not have a dispatcher.</remarks>
        public static Dispatcher Dispatcher { get; set; } // End of property

        /// <summary>
        /// Gets or sets the synchronization context of the main window.
        /// </summary>
        /// <value>The main window synchronization context.</value>
        /// <remarks>This should be set in the Loaded event handler for the application main window</remarks>
        public static SynchronizationContext MainWindowSynchronizationContext { get; set; } // End of property

        #endregion Private member variables and public properties

        /// <summary>
        /// Initializes a new instance of the <see cref="UIThread"/> class.
        /// </summary>
        /// <remarks>Must be created on the UI thread</remarks>
        public UIThread ()
        {
            _creationSyncContext = SynchronizationContext.Current;
            _creationThread = Thread.CurrentThread;
        } 

        /// <summary>
        /// Put the specified action on the UI thread.
        /// </summary>
        /// <param name="action">The action.</param>
        public void Put (Action action)
        {
            try
            {
                if (_creationThread == Thread.CurrentThread)
                {
                    // We are running on the creation thread, so just directly run the action
                    action();
                } 
                else if (Dispatcher != null)
                {
                    // We are running with our own dispatcher - because we're running in a unit test
                    if (Dispatcher.CheckAccess())
                    {
                        action();
                    } 
                    else
                    {
                        Dispatcher.BeginInvoke(action, DispatcherPriority.Normal, null);
                    } // End else
                } // End else if
                else if ((_creationSyncContext != null) || (MainWindowSynchronizationContext != null))
                {
                    if (_creationSyncContext == null)
                    {
                        _creationSyncContext = MainWindowSynchronizationContext;
                    } 

                    if (_creationSyncContext.GetType() == typeof(SynchronizationContext))
                    {
                        // We are running in free threaded context, so just directly run the action
                        action();
                    } 
                    else
                    {
                        // We are running in WindowsFormsSynchronizationContext or DispatcherSynchronizationContext, 
                        // so we should marshal the action back to the creation thread.
                        _creationSyncContext.Post(state => action(), null);
                    } // End else
                } // End else if
                else if ((System.Windows.Application.Current != null)
                    && (System.Windows.Application.Current.Dispatcher != null))
                {
                    if (System.Windows.Application.Current.Dispatcher.CheckAccess())
                    {
                        action();
                    } 
                    else
                    {
                        System.Windows.Application.Current.Dispatcher.BeginInvoke(action);
                    } // End else
                } // End else if
                else
                {
                    // If all else fails just directly run the action. This shouldn't happen.
                    action();
                } // End else
            } // End try

            catch (Exception )
            {
                
            } // End catch
        } 
    } 
} 

