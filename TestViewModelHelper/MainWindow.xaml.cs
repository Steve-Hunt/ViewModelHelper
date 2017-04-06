using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestViewModelHelper.ViewModels;

namespace TestViewModelHelper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _vm;
        public MainWindow()
        {
            InitializeComponent();
            _vm = new MainWindowViewModel();
            this.DataContext = _vm;
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            _vm.Calculate();
        }

        private void RevertButton_Click(object sender, RoutedEventArgs e)
        {
            _vm.Cancel();
        }


    }
}
