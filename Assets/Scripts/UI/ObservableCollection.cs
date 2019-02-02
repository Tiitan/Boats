using System.Collections.Generic;
using System.Collections.Specialized;

namespace UI
{
    public interface IReadOnlyObservableCollection<out T> : 
        IReadOnlyCollection<T>, INotifyCollectionChanged
    {}

    /// <summary>
    /// interface based ReadonlyObservableCollection 
    /// because wraping based .net implementation can't allow covariance
    /// </summary>
    public class ObservableCollection<T> :
        System.Collections.ObjectModel.ObservableCollection<T>, IReadOnlyObservableCollection<T>
    {
        public ObservableCollection() { }
        public ObservableCollection(IEnumerable<T> collection) : base(collection) { }
        public ObservableCollection(List<T> list) : base(list) { }
    }
}
