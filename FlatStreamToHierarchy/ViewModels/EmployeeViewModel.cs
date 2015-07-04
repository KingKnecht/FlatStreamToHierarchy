using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Input;
using DynamicData;
using DynamicData.Binding;
using DynamicData.Kernel;
using FlatStreamToHierarchy.Infrastructure;
using FlatStreamToHierarchy.Services.Dtos;

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

        public EmployeeViewModel(Node<EmployeeDto, int> node, Action<EmployeeViewModel> promoteAction, Action<EmployeeViewModel> sackAction, EmployeeViewModel parent = null)
        {
            Inferiors = new ObservableCollectionExtended<EmployeeViewModel>();
            Id = node.Key;
            Name = node.Item.Name;
            Depth = node.Depth;
            Parent = parent;
            BossId = node.Item.BossId; 
            Dto = node.Item;

            _promoteCommand = new Command(()=>promoteAction(this),()=>Parent.HasValue);
            _sackCommand = new Command(() => sackAction(this));

            var childrenLoader = new Lazy<IDisposable>(() => node.Children.Connect()
                                .Transform(e => new EmployeeViewModel(e, promoteAction, sackAction,this))
                                .Bind(Inferiors)
                                .DisposeMany()
                                .Subscribe());

            var employeesCount = node.Children.CountChanged
                .Select(count =>
                {
                    if (count == 0)
                        return "I am a at rock bottom";
                   
                     return count == 1 
                        ? "1 person reports to me"
                        : string.Format("{0} people reports to me", count);

                }).Subscribe(text=>EmployeeCountText=text);

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

            _cleanUp = Disposable.Create(() =>
            {
                disposer.Dispose();
                employeesCount.Dispose();
                if (childrenLoader.IsValueCreated)
                    childrenLoader.Value.Dispose();
            });
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public int Depth { get ; private set; }
        public int BossId { get; private set; }
        public EmployeeDto Dto { get; private set; }
        public Optional<EmployeeViewModel> Parent { get; private set; }
        public IObservableCollection<EmployeeViewModel> Inferiors { get; private set; }


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