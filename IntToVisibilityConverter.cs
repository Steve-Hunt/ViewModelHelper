using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ViewModelHelper
{
    /// <summary>
    /// Class: IntToVisibilityConverter
    /// </summary>
    /// <remarks>
    /// Converts between a <see cref="int"/> and a <see cref="Visibility"/>. A value
    /// greater than the threshold returns <see cref="Visibility.Visible"/>.
    /// </remarks>
    /// <example>This shows how to define the converter in XAML:
    /// <code>
    /// <labtech:IntToVisibilityConverter x:Key="IntToVisibilityConverter" InvisibleValue="Collapsed" Threshold="0"/>
    /// </code>
    /// </example>

    [ValueConversion(typeof(object), typeof(Visibility))]
    public class IntToVisibilityConverter :
        IValueConverter
    {
        /// <summary>
        /// Operator
        /// </summary>
        public enum Operator
        {
            /// <summary/>
            Equal,
            /// <summary/>
            LessThan,
            /// <summary/>
            GreaterThan,
            /// <summary/>
            LessThanOrEqual,
            /// <summary/>
            GreaterThanOrEqual
        } // End of enum

        /// <summary>
        /// Gets or sets the threshold.
        /// </summary>
        /// <value>The threshold.</value>
        public int Threshold
        {
            get;
            set;
        } // End of property

        public Operator OperatorType
        {
            get;
            set;
        } // End of property

        /// <summary>
        /// Gets or sets the invisible-value, indicating what to set the visibility
        /// to when the object is not visible. The default is Collapsed.
        /// </summary>
        /// <value>The show hidden.</value>
        public Visibility InvisibleValue
        {
            get;
            set;
        } // End of property

        /// <summary>
        /// Initializes a new instance of the <see cref="IntToVisibilityConverter"/> class.
        /// </summary>
        public
        IntToVisibilityConverter()
        {
            this.OperatorType = Operator.GreaterThan;
            this.InvisibleValue = Visibility.Collapsed;
        } 

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object
        Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (this.OperatorType)
            {
                case Operator.Equal:
                    return System.Convert.ToInt32(value) == this.Threshold ? Visibility.Visible : this.InvisibleValue;

                case Operator.LessThan:
                    return System.Convert.ToInt32(value) < this.Threshold ? Visibility.Visible : this.InvisibleValue;

                case Operator.GreaterThan:
                    return System.Convert.ToInt32(value) > this.Threshold ? Visibility.Visible : this.InvisibleValue;

                case Operator.LessThanOrEqual:
                    return System.Convert.ToInt32(value) <= this.Threshold ? Visibility.Visible : this.InvisibleValue;

                case Operator.GreaterThanOrEqual:
                    return System.Convert.ToInt32(value) >= this.Threshold ? Visibility.Visible : this.InvisibleValue;

                default:
                    break;
            } // End switch

            throw new InvalidOperationException("Invalid operator type " + this.OperatorType.ToString());
        } 

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        } 
    } 
} 

