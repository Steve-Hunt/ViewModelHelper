using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestViewModelHelper.Models;
using ViewModelHelper;

namespace TestViewModelHelper.ViewModels
{
    public class MainWindowViewModel : ViewModelCore
    {
        private TestModel _subject;
        private PropertyEx<int> _value1 = new PropertyEx<int>();
        private PropertyEx<int> _value2 = new PropertyEx<int>();


        public MainWindowViewModel(TestModel model)
        {
            Subject = model;
        }

        public MainWindowViewModel()
        {
            Subject = new TestModel()
            {
                Value1 = 2,
                Value2 = 3
            };
        }

        public TestModel Subject
        {
            get
            {
                return _subject;
            }
            private set
            {
                _subject = value;
                FirePropertyChanged(nameof(Subject));
            }
        }
        public int Value1
        {
            get
            {
                return _value1.IsDirty ? _value1.Value : _value1.SavedValue;
            }
            set
            {
                _value1.Value = value;
                FirePropertyChanged(nameof(Value1));
            }
        }

        public int Value2
        {
            get
            {
                return _value2.IsDirty? _value2.Value: _value2.SavedValue;
            }
            set
            {
                _value2.Value = value;
                FirePropertyChanged(nameof(Value2));
            }
        }

        public void Calculate()
        {
            _value1.CommitValue();
            Subject.Value1 = _value1.SavedValue;
            _value2.CommitValue();
            Subject.Value2 = _value2.SavedValue;
            QuickFirePropertyChanged(null);
        }

        public void Cancel()
        {
            _value1.RevertValue();
            _value2.RevertValue();
            QuickFirePropertyChanged(null);
        }

        public int Result
        {
            get
            {
                return Subject.Result;
            }
        }
    }
}
