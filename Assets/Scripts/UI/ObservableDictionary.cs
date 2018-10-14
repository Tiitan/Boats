using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Framework;

namespace UI
{
    public interface IReadOnlyObservableDictionary<in TKey, out TValue> : INotifyCollectionChanged
    {
        int Count { get; }

        bool ContainsKey(TKey key);

        TValue this[TKey key] { get; }

        IEnumerable<TValue> Values { get; }
    }

    [Serializable]
    public class ObservableDictionary<TKey, TValue> : SerializedDictionary<TKey, TValue>, IReadOnlyObservableDictionary<TKey, TValue>
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public override void Add(KeyValuePair<TKey, TValue> item)
        {
            base.Add(item);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item.Key));
        }

        public override void Clear()
        {
            var keys = Keys.ToList();
            base.Clear();
            if (keys.Count > 0)
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, keys));
        }

        public override bool Remove(KeyValuePair<TKey, TValue> item)
        {
            var result = base.Remove(item);
            if (result)
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item.Key));
            return result;
        }

        public override void Add(TKey key, TValue value)
        {
            base.Add(key, value);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, key));
        }

        public override bool Remove(TKey key)
        {
            var result = base.Remove(key);
            if (result)
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, key));
            return result;
        }

        public override TValue this[TKey key]
        {
            get { return base[key]; }
            set
            {
                var isContained = ContainsKey(key);
                base[key] = value;
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(isContained ? NotifyCollectionChangedAction.Replace : NotifyCollectionChangedAction.Add, key));
            }
        }

        IEnumerable<TValue> IReadOnlyObservableDictionary<TKey, TValue>.Values
        {
            get { return Values; }
        }
    }
}
