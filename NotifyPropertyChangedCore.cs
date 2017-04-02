using System.ComponentModel;
using System.Diagnostics;
using System;

namespace ViewModelHelper
{
    /// <summary>
    /// Class: NotifyPropertyChangedCore
    /// </summary>
    /// <remarks>Core/Base class that implements INotifyPropertyChanged</remarks>
    [Serializable]
    public class NotifyPropertyChangedCore :
        INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>

        [field: NonSerialized]
        public virtual event PropertyChangedEventHandler PropertyChanged = null;

        #endregion INotifyPropertyChanged Members

        #region Object lifetime

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyPropertyChangedCore"/> class.
        /// </summary>
        protected NotifyPropertyChangedCore ()
        {
        } 

        #endregion Object lifetime

        #region Protected Methods

        /// <summary>
        /// Fires the property changed event.
        /// </summary>
        /// <param name="propertyName">Name of the property; or null to indicate that all properties have changed.</param>
        protected virtual void FirePropertyChanged (string propertyName)
        {
            VerifyPropertyName(propertyName);

            PropertyChangedEventHandler eh = this.PropertyChanged;
            if (eh != null)
            {
                eh(this, new PropertyChangedEventArgs(propertyName));
            } 
        } 


        /// <summary>
        /// Fires the property changed event.
        /// </summary>
        /// <param name="PropertyName">Name of the property; or null to indicate that all properties have changed.</param>
        /// <remarks>This differs from FirePropertyChanged in that no check is made that the property name is valid.</remarks>
        protected virtual void QuickFirePropertyChanged (string PropertyName)
        {
            PropertyChangedEventHandler eh = this.PropertyChanged;
            if (eh != null)
            {
                eh(this, new PropertyChangedEventArgs(PropertyName));
            } 
        } 

        /// <summary>
        /// Verifies that a property exists given its name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        protected virtual void VerifyPropertyName (string propertyName)
        {
            if (!string.IsNullOrWhiteSpace(propertyName))
            {
                // Verify that the property name matches a real,  
                // public, instance property on this object.
                if (TypeDescriptor.GetProperties(this)[propertyName] == null)
                {
                    Debug.Fail("Invalid property name: " + propertyName);
                } 
            } 
        } 


        #endregion Protected Methods
    } 
} 
