using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using JetBrains.Annotations;
namespace Assets.MMORPG.Scripts.RPGGame.Data
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class SyncIDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        public delegate void SyncDictionaryChanged(Operation op, TKey key, TValue item);
        public event SyncDictionaryChanged Callback;
        protected readonly IDictionary<TKey, TValue> objects;

        public int Count => objects.Count;
        public bool IsReadOnly { get; private set; }


        public enum Operation : byte
        {
            OP_ADD,
            OP_CLEAR,
            OP_REMOVE,
            OP_SET
        }
        struct Change
        {
            internal Operation operation;
            internal TKey key;
            internal TValue item;
        }

        public void Reset()
        {
            IsReadOnly = false;
            objects.Clear();
        }

        protected virtual void SerializeKey(TKey item) { }
        protected virtual void SerializeItem(TValue item) { }

        public ICollection<TKey> Keys => objects.Keys;
        public ICollection<TValue> Values => objects.Values;


        protected SyncIDictionary(IDictionary<TKey, TValue> objects)
        {
            this.objects = objects;
        }
        void AddOperation(Operation op, TKey key, TValue item)
        {
            if (IsReadOnly)
            {
                throw new System.InvalidOperationException("SyncDictionaries can only be modified by the server");
            }

            Change change = new Change
            {
                operation = op,
                key = key,
                item = item
            };

            Callback?.Invoke(op, key, item);
        }
        public void Clear()
        {
            objects.Clear();
            AddOperation(Operation.OP_CLEAR, default, default);
        }

        public bool ContainsKey(TKey key) => objects.ContainsKey(key);

        public bool Remove(TKey key)
        {
            if (objects.TryGetValue(key, out TValue item) && objects.Remove(key))
            {
                AddOperation(Operation.OP_REMOVE, key, item);
                return true;
            }
            return false;
        }
        public TValue this[TKey i]
        {
            get => objects[i];
            set
            {
                if (ContainsKey(i))
                {
                    objects[i] = value;
                    AddOperation(Operation.OP_SET, i, value);
                }
                else
                {
                    objects[i] = value;
                    AddOperation(Operation.OP_ADD, i, value);
                }
            }
        }

        public bool TryGetValue(TKey key, out TValue value) => objects.TryGetValue(key, out value);

        public void Add(TKey key, TValue value)
        {
            objects.Add(key, value);
            AddOperation(Operation.OP_ADD, key, value);
        }

        public void Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return TryGetValue(item.Key, out TValue val) && EqualityComparer<TValue>.Default.Equals(val, item.Value);
        }
        public void CopyTo([NotNull] KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (arrayIndex < 0 || arrayIndex > array.Length)
            {
                throw new System.ArgumentOutOfRangeException(nameof(arrayIndex), "Array Index Out of Range");
            }
            if (array.Length - arrayIndex < Count)
            {
                throw new System.ArgumentException("SyncDictionary中的项数大于arrayIndex到目标数组末尾的可用空间");
            }

            int i = arrayIndex;
            foreach (KeyValuePair<TKey, TValue> item in objects)
            {
                array[i] = item;
                i++;
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            bool result = objects.Remove(item.Key);
            if (result)
            {
                AddOperation(Operation.OP_REMOVE, item.Key, item.Value);
            }
            return result;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => objects.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => objects.GetEnumerator();
    }
    public abstract class SyncDictionary<TKey, TValue> : SyncIDictionary<TKey, TValue>
    {
        protected SyncDictionary() : base(new Dictionary<TKey, TValue>())
        {
        }

        protected SyncDictionary(IEqualityComparer<TKey> eq) : base(new Dictionary<TKey, TValue>(eq))
        {
        }

        public new Dictionary<TKey, TValue>.ValueCollection Values => ((Dictionary<TKey, TValue>)objects).Values;

        public new Dictionary<TKey, TValue>.KeyCollection Keys => ((Dictionary<TKey, TValue>)objects).Keys;

        public new Dictionary<TKey, TValue>.Enumerator GetEnumerator() => ((Dictionary<TKey, TValue>)objects).GetEnumerator();
    }
}