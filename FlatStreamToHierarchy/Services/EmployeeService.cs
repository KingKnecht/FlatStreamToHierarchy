using System;
using System.Collections.Generic;
using System.Linq;
using DynamicData;

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

        public void Promote(EmployeeDto promtedDto, int  newBoss)
        {
            //simulate going to a service

            //update the cache with the emploee, 
            _employees.AddOrUpdate(new EmployeeDto(promtedDto.Id,promtedDto.Name,newBoss));
        }


        public void Sack(EmployeeDto sackEmp)
        {
            _employees.BatchUpdate(updater =>
            {
                //assign new boss to the workers of the sacked employee
                var workersWithNewBoss = updater.Items
                                    .Where(emp => emp.BossId == sackEmp.Id)
                                    .Select(dto => new EmployeeDto(dto.Id, dto.Name,  sackEmp.BossId))
                                    .ToArray();

                updater.AddOrUpdate(workersWithNewBoss);

                //get rid of the existing person
                updater.Remove(sackEmp.Id);
            });


        }

        private IEnumerable<EmployeeDto> CreateEmployees(int numberToLoad)
        {
            var random = new Random();

            return Enumerable.Range(1, numberToLoad)
                .Select(i =>
                {
                    var boss = i%1000 == 0 ? 0 : random.Next(0, i);
                    return new EmployeeDto(i, string.Format("Person {0}",i), boss);
                });
        }
    }
}
