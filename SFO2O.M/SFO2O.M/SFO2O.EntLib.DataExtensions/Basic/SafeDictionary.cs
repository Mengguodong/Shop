using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace SFO2O.EntLib.DataExtensions.Basic
{
    /// <summary>
    /// 提供线程安全的字典集合管理。
    /// </summary>
    public class SafeDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private IDictionary<TKey, TValue> _ObjectsDictionary;
        private object _SyncObject;

        /// <summary>
        /// 初始化一个对象容器。
        /// </summary>
        public SafeDictionary()
        {
            _SyncObject = new object();
            _ObjectsDictionary = new Dictionary<TKey, TValue>();
        }

        #region custom methods

        public TValue GetValue(TKey key, TValue defaultVal)
        {
            TValue val;
            if (this.TryGetValue(key, out val))
                return val;
            else
                return defaultVal;
        }

        public TValue GetValue(TKey key, Func<TValue> getter)
        {
            TValue val;
            if (this.TryGetValue(key, out val))
            {
                return val;
            }
            else
            {
                lock (_SyncObject)
                {
                    if (this.TryGetValue(key, out val))
                    {
                        return val;
                    }
                    // 
                    this.Add(key, getter());
                }
                return this[key];
            }
        }

        #endregion

        public void Add(TKey key, TValue value)
        {
            lock (_SyncObject)
            {
                Check.Valid(_ObjectsDictionary.ContainsKey(key), "Key:{0}，已经包含在集合中。", key);
                lock (_ObjectsDictionary)
                {
                    Check.Valid(_ObjectsDictionary.ContainsKey(key), "Key:{0}，已经包含在集合中。", key);
                    _ObjectsDictionary.Add(key, value);
                }
            }
        }

        public bool ContainsKey(TKey key)
        {
            return _ObjectsDictionary.ContainsKey(key);
        }

        public ICollection<TKey> Keys
        {
            get { return _ObjectsDictionary.Keys; }
        }

        public bool Remove(TKey key)
        {
            lock (_SyncObject)
            {
                lock (_ObjectsDictionary)
                {
                    return _ObjectsDictionary.Remove(key);
                }
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _ObjectsDictionary.TryGetValue(key, out value);
        }

        public ICollection<TValue> Values
        {
            get { return _ObjectsDictionary.Values; }
        }

        public TValue this[TKey key]
        {
            get
            {
                TValue val;
                if (this.TryGetValue(key, out val))
                    return val;
                else
                    return default(TValue);
            }
            set
            {
                lock (_SyncObject)
                {
                    lock (_ObjectsDictionary)
                    {
                        _ObjectsDictionary[key] = value;
                    }
                }
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            this.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            lock (_SyncObject)
            {
                lock (_ObjectsDictionary)
                {
                    _ObjectsDictionary.Clear();
                }
            }
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _ObjectsDictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _ObjectsDictionary.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _ObjectsDictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return _ObjectsDictionary.IsReadOnly; }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            lock (_SyncObject)
            {
                lock (_ObjectsDictionary)
                {
                    return _ObjectsDictionary.Remove(item);
                }
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _ObjectsDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _ObjectsDictionary.GetEnumerator();
        }
    }
}
