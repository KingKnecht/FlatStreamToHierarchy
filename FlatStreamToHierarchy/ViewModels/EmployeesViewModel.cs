using System;
using DynamicData;
using DynamicData.Binding;
using FlatStreamToHierarchy.Services;

namespace FlatStreamToHierarchy.ViewModels
{
    public class EmployeesViewModel : IDisposable
    {
        private readonly EmployeeService _employeeService;
        private readonly IObservableCollection<EmployeeViewModel> _employeeViewModels = new ObservableCollectionExtended<EmployeeViewModel>();
        private readonly IDisposable _cleanUp;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeesViewModel"/> class.
        /// </summary>
        /// <param name="employeeService">The employee service.</param>
        public EmployeesViewModel(EmployeeService employeeService)
        {
            _employeeService = employeeService;
            var stream = employeeService.Employees.Connect();

            //transform the data to a full nested tree
            //and transform into a fully recursive view model
            _cleanUp = stream.TransformToTree(employee => employee.BossId)
                .Transform(node => new EmployeeViewModel(node, Promote,Sack))
                .Sort(SortExpressionComparer<EmployeeViewModel>.Ascending(evm=>evm.Name))
                .Bind(_employeeViewModels)
                .DisposeMany()
                .Subscribe();    
        }

        private void Promote(EmployeeViewModel viewModel)
        {
            if (!viewModel.Parent.HasValue) return;
            _employeeService.Promote(viewModel.Dto,viewModel.Parent.Value.BossId);
        }

        private void Sack(EmployeeViewModel viewModel)
        {
            _employeeService.Sack(viewModel.Dto);
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
