using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlatStreamToHierarchy.Services.Dtos
{
    public class EmployeeDto
    {
        
        public EmployeeDto(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
        public int BossId { get; set; }

        public string Name { get; set; }
    }
}
