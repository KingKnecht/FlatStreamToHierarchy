using System;
using DynamicData;
using DynamicData.Binding;
using FlatStreamToHierarchy.Services;

namespace FlatStreamToHierarchy.ViewModels
{
    public class EmployeesViewModel : IDisposable
    {
        private readonly IObservableCollection<EmployeeViewModel> _employeeViewModels = new ObservableCollectionExtended<EmployeeViewModel>();
        private readonly IDisposable _cleanUp;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeesViewModel"/> class.
        /// </summary>
        /// <param name="employeeService">The employee service.</param>
        public EmployeesViewModel(EmployeeService employeeService)
        {
            var stream = employeeService.Employees.Connect();

            //transform the data to a full nested tree
            //and transform into a fully recursive view model
            _cleanUp = stream.TransformToTree(employee => employee.BossId)
                .Transform(node => new EmployeeViewModel(node, Promote,Sack))
                .Bind(_employeeViewModels)
                .DisposeMany()
                .Subscribe();    
        }

        private void Promote(EmployeeViewModel viewModel)
        {
            
        }

        private void Sack(EmployeeViewModel viewModel)
        {

        }

        public IObservableCollection<EmployeeViewModel> EmployeeViewModels
        {
            get { return _employeeViewModels; }
        }

        public void Dispose()
        {
            _cleanUp.Dispose();
        }
    }
}
