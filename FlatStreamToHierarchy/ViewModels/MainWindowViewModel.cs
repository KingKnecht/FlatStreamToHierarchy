using System.Diagnostics;
using System.Windows.Input;
using FlatStreamToHierarchy.Infrastructure;
using FlatStreamToHierarchy.Services;

namespace FlatStreamToHierarchy.ViewModels
{
    public class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            ShowInGitHubCommand = new Command(() => Process.Start("https://github.com/RolandPheasant"));
            Employees = new EmployeesViewModel(new EmployeeService());
        }

        public EmployeesViewModel Employees { get; private set; }
        public ICommand ShowInGitHubCommand { get; private set; }

    }
}