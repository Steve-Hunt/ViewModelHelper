using System;
using System.Threading;
using System.Windows;

namespace ViewModelHelper
{
    /// <summary>
    /// Class: ThreadedWindow
    /// </summary>
    public class ThreadedWindow
    {
        /// <summary>
        /// Shows the specified window.
        /// </summary>
        /// <param name="WindowToShow">The window to show.</param>
        public static void Show(Type WindowToShow)
        {
            Thread thread = new Thread(() =>
            {
                Window w = Activator.CreateInstance(WindowToShow) as Window;
                w.Show();
                System.Windows.Threading.Dispatcher.Run();
                w.Close();
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;
            thread.Start();
        } 

        /// <summary>
        /// Disposes the specified window.
        /// </summary>
        /// <param name="WindowToShow">The window to show.</param>
        public static void Dispose (Window WindowToShow)
        {
            if ((WindowToShow != null)
                && (WindowToShow.Dispatcher != null))
            {
                WindowToShow.Dispatcher.InvokeShutdown();
            } 
        } 
    } 
} 
