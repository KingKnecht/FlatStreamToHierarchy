using System;
using System.Collections.Generic;
using System.Linq;
using DynamicData;
using FlatStreamToHierarchy.Services.Dtos;

namespace FlatStreamToHierarchy.Services
{
    public class EmployeeService
    {
        private readonly SourceCache<EmployeeDto, int> _employees = new SourceCache<EmployeeDto, int>(x => x.Id); 

        public EmployeeService()
        {
            _employees.AddOrUpdate(CreateEmployees(25000));
        }

        public IObservableCache<EmployeeDto, int> Employees
        {
            get { return _employees.AsObservableCache(); }
        }

        public void Promote(EmployeeDto emp)
        {
            
        }


        public void Sack(EmployeeDto emp)
        {

        }



        private IEnumerable<EmployeeDto> CreateEmployees(int numberToLoad)
        {
            var random = new Random();

            return Enumerable.Range(1, numberToLoad)
                .Select(i =>
                {
                    var boss = i%1000 == 0 ? 0 : random.Next(0, i);
                    return new EmployeeDto(i){Name = string.Format("Person {0}",i),BossId = boss};
                });
        }
    }
}
