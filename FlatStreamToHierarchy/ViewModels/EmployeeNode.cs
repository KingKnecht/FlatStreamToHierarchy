using System;
using DynamicData;
using FlatStreamToHierarchy.Services.Dtos;

namespace FlatStreamToHierarchy.ViewModels
{
    public class EmployeeNode : IEquatable<EmployeeNode>
    {
        private readonly EmployeeDto _employeeDto;
        private readonly IObservable<IChangeSet<EmployeeNode, int>> _children;

        public EmployeeNode(EmployeeDto employeeDto, Func<int,IObservable<IChangeSet<EmployeeDto, int>>> childrenFactory)
        {
            if (employeeDto == null) throw new ArgumentNullException("employeeDto");
            if (childrenFactory == null) throw new ArgumentNullException("childrenFactory");

            _employeeDto = employeeDto;
            _children = childrenFactory(employeeDto.Id)
                .Transform(dto => new EmployeeNode(dto, childrenFactory));
        }

        public int Id
        {
            get { return _employeeDto.Id; }
        }

        public int BossId
        {
            get { return _employeeDto.BossId; }
        }

        public string Name
        {
            get { return _employeeDto.Name; }
        }

        public IObservable<IChangeSet<EmployeeNode, int>> Children
        {
            get { return _children; }
        }

        #region Equality Members

        public bool Equals(EmployeeNode other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(_employeeDto, other._employeeDto);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EmployeeNode) obj);
        }

        public override int GetHashCode()
        {
            return (_employeeDto != null ? _employeeDto.GetHashCode() : 0);
        }

        public static bool operator ==(EmployeeNode left, EmployeeNode right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EmployeeNode left, EmployeeNode right)
        {
            return !Equals(left, right);
        }

        #endregion

        public override string ToString()
        {
            return string.Format("Id: {0}, BossId: {1}, Name: {2}", Id, BossId, Name);
        }
    }
}