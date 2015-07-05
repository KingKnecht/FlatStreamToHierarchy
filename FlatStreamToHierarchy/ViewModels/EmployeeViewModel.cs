using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Input;
using DynamicData;
using DynamicData.Binding;
using DynamicData.Kernel;
using FlatStreamToHierarchy.Infrastructure;
using FlatStreamToHierarchy.Services;

namespace FlatStreamToHierarchy.ViewModels
{
    public class EmployeeViewModel:AbstractNotifyPropertyChanged, IDisposable, IEquatable<EmployeeViewModel>
    {
        private readonly IDisposable _cleanUp;
        private bool _isExpanded;
        private bool _isSelected;
        private readonly Command _promoteCommand;
        private readonly Command _sackCommand;
        private string _employeeCountText;
        private readonly int _depth;
        private readonly int _bossId;
        private readonly EmployeeDto _dto;
        private readonly Optional<EmployeeViewModel> _parent;
        private readonly IObservableCollection<EmployeeViewModel> _inferiors;
        private readonly int _id;
        private readonly string _name;

        public EmployeeViewModel(Node<EmployeeDto, int> node, Action<EmployeeViewModel> promoteAction, Action<EmployeeViewModel> sackAction, EmployeeViewModel parent = null)
        {
            _inferiors = new ObservableCollectionExtended<EmployeeViewModel>();
            _id = node.Key;
            _name = node.Item.Name;
            _depth = node.Depth;
            _parent = parent;
            _bossId = node.Item.BossId; 
            _dto = node.Item;

            _promoteCommand = new Command(()=>promoteAction(this),()=>Parent.HasValue);
            _sackCommand = new Command(() => sackAction(this));

            //use laxy loading 
            var childrenLoader = new Lazy<IDisposable>(() => node.Children.Connect()
                                .Transform(e => new EmployeeViewModel(e, promoteAction, sackAction,this))
                                .Bind(Inferiors)
                                .DisposeMany()
                                .Subscribe());
            
            var disposer = new SingleAssignmentDisposable();
            if (!Parent.HasValue)
            {
                //force loading now
                var x = childrenLoader.Value;
            }
            else
            {
                //load children when the parent has expanded
                disposer.Disposable = Parent.Value.ObservePropertyValue(This => This.IsExpanded).Value()
                    .Where(isExpanded => isExpanded)
                    .Take(1)
                    .Subscribe(_ =>
                    {
                        //force lazy loading
                        var x = childrenLoader.Value;
                    });
            }

            //create some display text based on the number of employees
            var employeesCount = node.Children.CountChanged
                .Select(count =>
                {
                    if (count == 0)
                        return "I am a at rock bottom";

                    return count == 1
                       ? "1 person reports to me"
                       : string.Format("{0} people reports to me", count);

                }).Subscribe(text => EmployeeCountText = text);

            _cleanUp = Disposable.Create(() =>
            {
                disposer.Dispose();
                employeesCount.Dispose();
                if (childrenLoader.IsValueCreated)
                    childrenLoader.Value.Dispose();
            });
        }

        public int Id
        {
            get { return _id; }
        }

        public string Name
        {
            get { return _name; }
        }

        public int Depth
        {
            get { return _depth; }
        }

        public int BossId
        {
            get { return _bossId; }
        }

        public EmployeeDto Dto
        {
            get { return _dto; }
        }

        public Optional<EmployeeViewModel> Parent
        {
            get { return _parent; }
        }

        public IObservableCollection<EmployeeViewModel> Inferiors
        {
            get { return _inferiors; }
        }
        
        public ICommand PromoteCommand
        {
            get { return _promoteCommand; }
        }

        public ICommand SackCommand
        {
            get { return _sackCommand; }
        }

        public string EmployeeCountText
        {
            get { return _employeeCountText; }
            set { SetAndRaise(ref _employeeCountText, value); }
        }
        
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set {SetAndRaise(ref _isExpanded,value); }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetAndRaise(ref _isSelected, value); }
        }
        
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