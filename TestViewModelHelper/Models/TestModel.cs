using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelHelper;

namespace TestViewModelHelper.Models
{
    public class TestModel : NotifyPropertyChangedCore
    {
        private int _value1;
        private int _value2;
        private int _result;

        public TestModel()
        {
        }

        public int Value1
        {
            get
            {
                return _value1;
            }
            set
            {
                if (!Equals(_value1, value))
                {
                    _value1 = value;
                    FirePropertyChanged(nameof(Value1));
                    Evaluate();
                }
            }
        }

        public int Value2
        {
            get
            {
                return _value2;
            }
            set
            {
                if (!Equals(_value2, value))
                {
                    _value2 = value;
                    FirePropertyChanged(nameof(Value2));
                    Evaluate();
                }
            }
        }

        public int Result
        {
            get
            {
                return _result;
            }
            private set
            {
                if (!Equals(_result, value))
                {
                    _result = value;
                    FirePropertyChanged(nameof(Result));
                }
            }
        }

        private void Evaluate()
        {
            Result = Value1 + Value2;
        }

    }
}
