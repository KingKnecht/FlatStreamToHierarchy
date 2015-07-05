using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using FlatStreamToHierarchy.Infrastructure;
using FlatStreamToHierarchy.Services;
using FlatStreamToHierarchy.ViewModels;

namespace FlatStreamToHierarchy
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly MainWindowViewModel _viewModel = new MainWindowViewModel();


        public MainWindow()
        {
            InitializeComponent();

            DataContext = _viewModel;
        }
    }
}
