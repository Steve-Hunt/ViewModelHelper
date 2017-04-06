using System;

namespace ViewModelHelper
{
    public class PropertyEx<T> : NotifyPropertyChangedCore
    {
        private T _value;
        private T _savedValue;

        public bool IsDirty { get; private set; }


        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (!Equals(value, _value))
                {
                    _value = value;
                    IsDirty = true;
                    FirePropertyChanged(nameof(Value));
                    FirePropertyChanged(nameof(IsDirty));
                }
            }
        }

        public T SavedValue
        {
            get
            {
                return _savedValue;
            }
            private set
            {
                if (!Equals(value, _savedValue))
                {
                    _savedValue = value;
                    IsDirty = false;
                }
            }
        }

        public Tuple<T,T,bool> AsTuple
        {
            get
            {
                return Tuple.Create<T, T, bool>(Value, SavedValue, IsDirty);
            }
            set
            {
                Value = value.Item1;
                SavedValue = value.Item2;
                IsDirty = value.Item3;
            }
        }

        public void CommitValue()
        {
            SavedValue = Value;
        }

        public void RevertValue()
        {
            Value = SavedValue;
            IsDirty = false;
            FirePropertyChanged(nameof(IsDirty));
        }


    }
}
