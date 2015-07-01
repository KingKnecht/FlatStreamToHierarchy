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

        public EmployeesViewModel(EmployeeService employeeService)
        {
            var stream = employeeService.Employees.Connect();
            
            /*
              split node from vm as I will abstract
              make a new heirachal operator in dynamic data 
              and optimise
             */
            var nodes = stream.Filter(dto => dto.BossId == 0)
                .Transform(dto => new EmployeeNode(dto, id => stream.Filter(e => e.BossId == id)))
                .AsObservableCache();

            //load recursive view model
            _cleanUp = nodes.Connect()
                .Transform(e => new EmployeeViewModel(e))
                .Bind(_employeeViewModels)
                .DisposeMany()
                .Subscribe();    
        
            //other notes
            //1. I implenented equality members using the id fields as some dd operators depend on it
            //2. The implementation of nodes using .Filter (above) may be slow for large data sets. I will sort this out when I do the hierachal operator
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
