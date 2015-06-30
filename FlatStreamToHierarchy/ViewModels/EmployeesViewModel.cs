using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using DynamicData.Binding;
using FlatStreamToHierarchy.Services;
using FlatStreamToHierarchy.Services.Dtos;

namespace FlatStreamToHierarchy.ViewModels
{
    public class EmployeesViewModel
    {
        private readonly IObservableCollection<EmployeeViewModel> _employeeViewModels = new ObservableCollectionExtended<EmployeeViewModel>();

        public EmployeesViewModel(EmployeeService employeeService)
        {
            employeeService.Employees.Connect()
                .Transform(e => new EmployeeViewModel(e))
                .Bind(_employeeViewModels)
                .DisposeMany()
                .Subscribe();

            //I guess after having a cache for my viewmodels the magic will happen here....
            //At the moment the TreeView is showing a flat list.

            
            //
            //This is only for testing my TreeView XAML stuff to make sure it's able to display a tree-like structure.
            //Comment out to see 2 expandable nodes in TreeView.
            //

            //var boss = new EmployeeViewModel(1, 0, "Boss");
            //boss.Inferiors.Add(new EmployeeViewModel(2, 1, "Employee1"));
            //boss.Inferiors.Add(new EmployeeViewModel(3, 1, "Employee2"));

            //var anotherBoss = new EmployeeViewModel(4, 0, "anotherBoss");
            //anotherBoss.Inferiors.Add(new EmployeeViewModel(5, 4, "Employee3"));
            //anotherBoss.Inferiors.Add(new EmployeeViewModel(6, 4, "Employee4"));

            //EmployeeViewModels.Add(boss);
            //EmployeeViewModels.Add(anotherBoss);

            
        }

        public IObservableCollection<EmployeeViewModel> EmployeeViewModels
        {
            get { return _employeeViewModels; }
        }
    }

    public class EmployeeViewModel
    {
        public EmployeeViewModel(EmployeeDto employeeDto)
        {
            Id = employeeDto.Id;
            BossId = employeeDto.BossId;
            Name = employeeDto.Name;
        }

        //For testing Ctor...I don't need the EmployeeDto
        public EmployeeViewModel(int id, int bossId, string name)
        {
            Id = id;
            BossId = bossId;
            Name = name;
            Inferiors = new ObservableCollectionExtended<EmployeeViewModel>();
        }

        public int Id { get; set; }
        public int BossId { get; set; }
        public string Name { get; set; }

        public IObservableCollection<EmployeeViewModel> Inferiors { get; set; }
    }
}
