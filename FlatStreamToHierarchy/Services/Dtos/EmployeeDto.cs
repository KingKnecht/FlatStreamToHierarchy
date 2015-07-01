using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlatStreamToHierarchy.Services.Dtos
{
    public class EmployeeDto : IEquatable<EmployeeDto>
    {
        
        public EmployeeDto(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
        public int BossId { get; set; }
        public string Name { get; set; }

        #region Equality Members

        public bool Equals(EmployeeDto other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EmployeeDto) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public static bool operator ==(EmployeeDto left, EmployeeDto right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EmployeeDto left, EmployeeDto right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}
