using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    [Serializable]
    public class SerializedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        private readonly IDictionary<TKey, TValue> _internalDictionary = new Dictionary<TKey, TValue>();

        public int Count => _internalDictionary.Count;
        public bool IsReadOnly => _internalDictionary.IsReadOnly;
        public ICollection<TKey> Keys => _internalDictionary.Keys;

        public ICollection<TValue> Values => _internalDictionary.Values;

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _internalDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public virtual void Add(KeyValuePair<TKey, TValue> item)
        {
            _internalDictionary.Add(item);
        }

        public virtual void Clear()
        {
            _internalDictionary.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _internalDictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _internalDictionary.CopyTo(array, arrayIndex);
        }

        public virtual bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return _internalDictionary.Remove(item);
        }

        public virtual void Add(TKey key, TValue value)
        {
            _internalDictionary.Add(key, value);
        }

        public bool ContainsKey(TKey key)
        {
            return _internalDictionary.ContainsKey(key);
        }

        public virtual bool Remove(TKey key)
        {
            return _internalDictionary.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _internalDictionary.TryGetValue(key, out value);
        }

        public virtual TValue this[TKey key]
        {
            get { return _internalDictionary[key]; }
            set { _internalDictionary[key] = value; }
        }

        #region Unity serialization
        // TODO: fix serialization

        [Serializable]
        private class SerializedkeyValuePair
        {
            public TKey Key;
            public TValue Value;
        }

        [SerializeField] [Header("Items")] private List<SerializedkeyValuePair> _listSerializedDictionary;

        public void OnBeforeSerialize()
        {
            _listSerializedDictionary.Clear();
            foreach (var keyValuePair in _internalDictionary)
                _listSerializedDictionary.Add(new SerializedkeyValuePair{ Key = keyValuePair.Key, Value = keyValuePair.Value});
        }

        public void OnAfterDeserialize()
        {
            _internalDictionary.Clear();
            foreach (var serializedkeyValuePair in _listSerializedDictionary)
                _internalDictionary.Add(serializedkeyValuePair.Key, serializedkeyValuePair.Value);
        }

        #endregion
    }
}
