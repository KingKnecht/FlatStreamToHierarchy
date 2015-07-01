using System;
using System.Collections.Generic;
using DynamicData;
using FlatStreamToHierarchy.Services.Dtos;

namespace FlatStreamToHierarchy.Services
{
    public class EmployeeService
    {

        private readonly SourceCache<EmployeeDto, int> _employees = new SourceCache<EmployeeDto, int>(x => x.Id); 

        public EmployeeService()
        {
            foreach (var employeeDto in CreateEmployees())
            {
                _employees.AddOrUpdate(employeeDto);
            }
        }

        public IObservableCache<EmployeeDto, int> Employees
        {
            get { return _employees.AsObservableCache(); }
        }

        private IEnumerable<EmployeeDto> CreateEmployees()
        {
            yield return new EmployeeDto(1)
            {
                BossId = 0,
                Name = "Employee1, has no boss. Id 0 does not exist"
            };

            yield return new EmployeeDto(2)
            {
                BossId = 1,
                Name = "Employee2"
            };

            yield return new EmployeeDto(3)
            {
                BossId = 1,
                Name = "Employee3"
            };

            yield return new EmployeeDto(4)
            {
                BossId = 3,
                Name = "Employee4, boss is employee3"
            };

            yield return new EmployeeDto(5)
            {
                BossId = 4,
                Name = "Employee5, boss is employee4"
            };

            yield return new EmployeeDto(6)
            {
                BossId = 2,
                Name = "Employee6, boss is employee2"
            };

            yield return new EmployeeDto(7)
            {
                BossId = 0,
                Name = "Employee7, has no boss. Id 0 does not exist"
            };


        }
    }
}
