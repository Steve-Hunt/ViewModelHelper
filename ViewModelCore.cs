using System.Diagnostics;
using System.Runtime.Serialization;
using System;
using System.Threading.Tasks;
using System.Threading;


namespace ViewModelHelper
{
    /// <summary>
    /// Class: ViewModelCore
    /// </summary>
    /// <remarks>Core/Base ViewModel class</remarks>
    [Serializable]
    public class ViewModelCore : NotifyPropertyChangedCore
    {
        #region Protected member variables

        /// <summary>
        /// The UI thread synchronizer
        /// </summary>
        [NonSerialized]
        protected UIThread _uiThread = null;

        #endregion Protected member variables

        #region Object lifetime

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelCore"/> class.
        /// </summary>
        protected ViewModelCore ()
        {
            _uiThread = new UIThread();
        } 

        /// <summary>
        /// Called when the object is de-serialized.
        /// </summary>
        /// <param name="context">The context.</param>
        [OnDeserialized]
        private void OnDeserialized (StreamingContext context)
        {
            if (_uiThread == null)
            {
                _uiThread = new UIThread();
            } 

        } 

        #endregion Object lifetime

        #region Protected Methods



        /// <summary>
        /// Run the command as a background task.
        /// </summary>
        /// <param name="command">The command method.</param>
        /// <param name="result">The result method (may be null).</param>
        /// <param name="failure">The failure method (may be null).</param>
        public static Task RunTask (Action command, Action result = null, Action<Exception> failure = null)
        {
            Task t = Task.Factory.StartNew(command);

            t.ContinueWith(success =>
            {
                if (result != null)
                {
                    result();
                } 
            }, TaskContinuationOptions.NotOnFaulted);

            t.ContinueWith(fail =>
            {
                Debug.Assert(fail.Exception != null, "fail.Exception != null");

                if (failure != null)
                {
                    failure(fail.Exception.InnerException);
                } 
            }, TaskContinuationOptions.OnlyOnFaulted);

            return t;
        } 

        /// <summary>
        /// Run the command as a background task.
        /// </summary>
        /// <param name="command">The command method.</param>
        /// <param name="result">The result method (may be null).</param>
        /// <param name="failure">The failure method (may be null).</param>
        protected virtual Task RunAsTask (Action command, Action result = null, Action<Exception> failure = null)
        {
            Task t = Task.Factory.StartNew(command);

            t.ContinueWith(success =>
            {
                if (result != null)
                {
                    result();
                } 
            }, TaskContinuationOptions.NotOnFaulted);

            t.ContinueWith(fail =>
            {
                Debug.Assert(fail.Exception != null, "fail.Exception != null");
                if (failure != null)
                {
                    failure(fail.Exception.InnerException);
                } 
            }, TaskContinuationOptions.OnlyOnFaulted);

            return t;
        } 

        /// <summary>
        /// Runs as task.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="command">The command.</param>
        /// <param name="result">The result.</param>
        /// <param name="failure">The failure.</param>
        /// <returns></returns>
        protected virtual Task RunAsTask (CancellationToken cancellationToken, Action<CancellationToken> command, Action<CancellationToken> result = null, Action<Exception> failure = null)
        {
            Task t = Task.Factory.StartNew(() => command(cancellationToken));

            t.ContinueWith(success =>
            {
                if (result != null)
                {
                    result(cancellationToken);
                } 
            }, TaskContinuationOptions.NotOnFaulted);

            t.ContinueWith(fail =>
            {
                if (fail.Exception != null)
                {
                    if (failure != null)
                    {
                        failure(fail.Exception.InnerException);
                    } 
                }
            }, TaskContinuationOptions.OnlyOnFaulted);

            return t;
        } 

        /// <summary>
        /// Run the command as a background task.
        /// </summary>
        /// <typeparam name="T">The type of the result of the command</typeparam>
        /// <param name="command">The command method.</param>
        /// <param name="result">The result method (may be null).</param>
        /// <param name="failure">The failure method (may be null).</param>
        /// <remarks>The result of executing the command is passed to the result method.</remarks>
        protected virtual void RunAsTask<T> (Func<T> command, Action<T> result = null, Action<Exception> failure = null)
        {
            T commandResult = default(T);

            Task t = Task.Factory.StartNew(() => commandResult = command());

            t.ContinueWith(success =>
            {
                if (result != null)
                {
                    result(commandResult);
                } 
            }, TaskContinuationOptions.NotOnFaulted);

            t.ContinueWith(fail =>
            {
                if (fail.Exception != null)
                {
                    if (failure != null)
                    {
                        failure(fail.Exception.InnerException);
                    } 
                }
            }, TaskContinuationOptions.OnlyOnFaulted);
        } 

        /// <summary>
        /// Runs as task.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="command">The command.</param>
        /// <param name="result">The result.</param>
        /// <param name="failure">The failure.</param>
        protected virtual void RunAsTask<T> (CancellationToken cancellationToken, Func<CancellationToken, T> command, Action<CancellationToken, T> result = null, Action<Exception> failure = null)
        {
            T commandResult = default(T);

            Task t = Task.Factory.StartNew(() => commandResult = command(cancellationToken));

            t.ContinueWith(success =>
            {
                if (result != null)
                {
                    result(cancellationToken, commandResult);
                } 
            }, TaskContinuationOptions.NotOnFaulted);

            t.ContinueWith(fail =>
            {
                if (fail.Exception != null)
                {
                    if (failure != null)
                    {
                        failure(fail.Exception.InnerException);
                    } 
                }
            }, TaskContinuationOptions.OnlyOnFaulted);
        } 

        /// <summary>
        /// Run the command as a background task and return control to the UI thread.
        /// </summary>
        /// <param name="command">The command method.</param>
        /// <param name="result">The result method (may be null).</param>
        /// <param name="failure">The failure method (may be null).</param>
        protected virtual Task RunAsUITask (Action command, Action result = null, Action<Exception> failure = null)
        {
            Task t = Task.Factory.StartNew(command);

            t.ContinueWith(success =>
            {
                if ((result != null)
                    && (_uiThread != null))
                {
                    _uiThread.Put(result);
                } 
            }, TaskContinuationOptions.NotOnFaulted);

            t.ContinueWith(fail =>
            {
                if (fail.Exception != null)
                {
                    if ((failure != null)
                        && (_uiThread != null))
                    {
                        _uiThread.Put(() => failure(fail.Exception.InnerException));
                    } 
                }
            }, TaskContinuationOptions.OnlyOnFaulted);

            return t;
        } 

        /// <summary>
        /// Runs as UI task.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="command">The command.</param>
        /// <param name="result">The result.</param>
        /// <param name="failure">The failure.</param>
        protected virtual void RunAsUITask (CancellationToken cancellationToken, Action<CancellationToken> command, Action<CancellationToken> result = null, Action<Exception> failure = null)
        {
            Task t = Task.Factory.StartNew(() => command(cancellationToken), cancellationToken);

            t.ContinueWith(success =>
            {
                if ((result != null)
                    && (_uiThread != null))
                {
                    _uiThread.Put(() => result(cancellationToken));
                } 
            },
            cancellationToken,
            TaskContinuationOptions.NotOnFaulted,
            TaskScheduler.Default);

            t.ContinueWith(fail =>
            {
                if (fail.Exception != null)
                {
                    if ((failure != null)
                        && (_uiThread != null))
                    {
                        _uiThread.Put(() => failure(fail.Exception.InnerException));
                    } 
                }
            }, TaskContinuationOptions.OnlyOnFaulted);
        } 

        /// <summary>
        /// Run the command as a background task and return control to the UI thread.
        /// </summary>
        /// <typeparam name="T">The type of the result of the command</typeparam>
        /// <param name="command">The command method.</param>
        /// <param name="result">The result method (may be null).</param>
        /// <param name="failure">The failure method (may be null).</param>
        /// <returns></returns>
        /// <remarks>The result of executing the command is passed to the result method.</remarks>
        protected virtual Task RunAsUITask<T> (Func<T> command, Action<T> result = null, Action<Exception> failure = null)
        {
            T commandResult = default(T);

            Task t = Task.Factory.StartNew(() =>
            {
                commandResult = command();
            });

            t.ContinueWith(success =>
            {
                if ((result != null)
                    && (_uiThread != null))
                {
                    _uiThread.Put(() => result(commandResult));
                } 
            }, TaskContinuationOptions.NotOnFaulted);

            t.ContinueWith(fail =>
            {
                if (fail.Exception != null)
                {
                    if ((failure != null)
                         && (_uiThread != null))
                    {
                        _uiThread.Put(() => failure(fail.Exception.InnerException));
                    } 
                }
            }, TaskContinuationOptions.OnlyOnFaulted);

            return t;
        } 

        /// <summary>
        /// Run the command as a background task and return control to the UI thread.
        /// </summary>
        /// <typeparam name="T">The type of the result of the command</typeparam>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="command">The command method.</param>
        /// <param name="result">The result method (may be null).</param>
        /// <param name="failure">The failure method (may be null).</param>
        /// <returns></returns>
        /// <remarks>The result of executing the command is passed to the result method.</remarks>
        protected virtual Task RunAsUITask<T> (CancellationToken cancellationToken, Func<CancellationToken, T> command, Action<CancellationToken, T> result = null, Action<Exception> failure = null)
        {
            T commandResult = default(T);

            Task t = Task.Factory.StartNew(() => commandResult = command(cancellationToken));

            t.ContinueWith(success =>
            {
                if ((result != null)
                    && (_uiThread != null))
                {
                    _uiThread.Put(() => result(cancellationToken, commandResult));
                } 
            }, TaskContinuationOptions.NotOnFaulted);

            t.ContinueWith(fail =>
            {
                if (fail.Exception != null)
                {


                    if ((failure != null)
                        && (_uiThread != null))
                    {
                        _uiThread.Put(() => failure(fail.Exception.InnerException));
                    } 
                }
            }, TaskContinuationOptions.OnlyOnFaulted);

            return t;
        } 

        #endregion Protected Methods
    } 
} 

