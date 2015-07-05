using System;

namespace FlatStreamToHierarchy.Services
{
    public class EmployeeDto : IEquatable<EmployeeDto>
    {
        private readonly int _id;
        private readonly int _bossId;
        private readonly string _name;

        public EmployeeDto(int id, string name, int boss)
        {
            _id = id;
            _name = name;
            _bossId = boss;
        }

        public int Id
        {
            get { return _id; }
        }

        public int BossId
        {
            get { return _bossId; }
        }

        public string Name
        {
            get { return _name; }
        }

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
