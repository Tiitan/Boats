using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace UI
{
    public interface IReadOnlyObservableDictionary<in TKey, out TValue> : INotifyCollectionChanged
    {
        int Count { get; }

        bool ContainsKey(TKey key);

        TValue this[TKey key] { get; }

        IEnumerable<TValue> Values { get; }
    }

    public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyObservableDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> _internalDictionary = new Dictionary<TKey, TValue>();

        public int Count => _internalDictionary.Count;
        public bool IsReadOnly => _internalDictionary.IsReadOnly;
        public ICollection<TKey> Keys => _internalDictionary.Keys;

        public ICollection<TValue> Values => _internalDictionary.Values;

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _internalDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            _internalDictionary.Add(item);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item.Key));
        }

        public void Clear()
        {
            var keys = _internalDictionary.Keys;
            _internalDictionary.Clear();
            if (keys.Count > 0)
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, keys.ToList()));
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _internalDictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _internalDictionary.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            var result = _internalDictionary.Remove(item);
            if (result)
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item.Key));
            return result;
        }

        public void Add(TKey key, TValue value)
        {
            _internalDictionary.Add(key, value);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, key));
        }

        public bool ContainsKey(TKey key)
        {
            return _internalDictionary.ContainsKey(key);
        }

        public bool Remove(TKey key)
        {
            var result = _internalDictionary.Remove(key);
            if (result)
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, key));
            return _internalDictionary.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _internalDictionary.TryGetValue(key, out value);
        }

        public TValue this[TKey key]
        {
            get { return _internalDictionary[key]; }
            set
            {
                var isContained = _internalDictionary.ContainsKey(key);
                _internalDictionary[key] = value;
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(isContained ? NotifyCollectionChangedAction.Replace : NotifyCollectionChangedAction.Add, key));
            }
        }

        IEnumerable<TValue> IReadOnlyObservableDictionary<TKey, TValue>.Values
        {
            get { return Values; }
        }
    }
}
