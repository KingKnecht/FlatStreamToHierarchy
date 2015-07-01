using System;
using DynamicData;
using DynamicData.Binding;

namespace FlatStreamToHierarchy.ViewModels
{
    public class EmployeeViewModel: IDisposable, IEquatable<EmployeeViewModel>
    {
        private readonly IDisposable _cleanUp;

        public EmployeeViewModel(EmployeeNode node)
        {
            Inferiors = new ObservableCollectionExtended<EmployeeViewModel>();
            Id = node.Id;
            BossId = node.BossId;
            Name = node.Name;

            _cleanUp = node.Children
                .Transform(e => new EmployeeViewModel(e))
                .Bind(Inferiors)
                .DisposeMany()
                .Subscribe();

        }

        public int Id { get; private set; }
        public int BossId { get; private set; }
        public string Name { get; private set; }
        public IObservableCollection<EmployeeViewModel> Inferiors { get; private set; }

        #region Equality Members

        public bool Equals(EmployeeViewModel other)
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
            return Equals((EmployeeViewModel) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public static bool operator ==(EmployeeViewModel left, EmployeeViewModel right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EmployeeViewModel left, EmployeeViewModel right)
        {
            return !Equals(left, right);
        }

        #endregion

        
        public void Dispose()
        {
            _cleanUp.Dispose();
        }
    }
}