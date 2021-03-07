using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

// 自定义的一个同步列表类,增加了一些方便列表操作的方法
// 主要有SyncListChanged callback
// 比如当更换装备时，可调用角色装备组件OnEquipmentChanged的委托方法

[EditorBrowsable(EditorBrowsableState.Never)]
public abstract class SyncList<T> : IList<T>, IReadOnlyList<T>
{
    public delegate void SyncListChanged(Operation op, int itemIndex, T oldItem, T newItem);
    public event SyncListChanged Callback;

    readonly IList<T> objects;
    readonly IEqualityComparer<T> comparer;

    public int Count => objects.Count;
    public bool IsReadOnly { get; private set; }

    public enum Operation : byte
    {
        OP_ADD,
        OP_CLEAR,
        OP_INSERT,
        OP_REMOVEAT,
        OP_SET
    }

    protected SyncList(IEqualityComparer<T> comparer = null)
    {
        this.comparer = comparer ?? EqualityComparer<T>.Default;
        objects = new List<T>();
    }

    protected SyncList(IList<T> objects, IEqualityComparer<T> comparer = null)
    {
        this.comparer = comparer ?? EqualityComparer<T>.Default;
        this.objects = objects;
    }

    public void Reset()
    {
        IsReadOnly = false;
        objects.Clear();
    }

    void AddOperation(Operation op, int itemIndex, T oldItem, T newItem)
    {
        if (IsReadOnly)
        {
            throw new InvalidOperationException("Synclists can only be modified at the server");
        }

        Callback?.Invoke(op, itemIndex, oldItem, newItem);
    }

    public void Add(T item)
    {
        objects.Add(item);
        AddOperation(Operation.OP_ADD, objects.Count - 1, default, item);
    }

    public void Clear()
    {
        objects.Clear();
        AddOperation(Operation.OP_CLEAR, 0, default, default);
    }

    public bool Contains(T item) => IndexOf(item) >= 0;

    public void CopyTo(T[] array, int index) => objects.CopyTo(array, index);

    public int IndexOf(T item)
    {
        for (int i = 0; i < objects.Count; ++i)
            if (comparer.Equals(item, objects[i]))
                return i;
        return -1;
    }

    public int FindIndex(Predicate<T> match)
    {
        for (int i = 0; i < objects.Count; ++i)
            if (match(objects[i]))
                return i;
        return -1;
    }

    public T Find(Predicate<T> match)
    {
        int i = FindIndex(match);
        return (i != -1) ? objects[i] : default;
    }

    public List<T> FindAll(Predicate<T> match)
    {
        List<T> results = new List<T>();
        for (int i = 0; i < objects.Count; ++i)
            if (match(objects[i]))
                results.Add(objects[i]);
        return results;
    }

    public void Insert(int index, T item)
    {
        objects.Insert(index, item);
        AddOperation(Operation.OP_INSERT, index, default, item);
    }

    public bool Remove(T item)
    {
        int index = IndexOf(item);
        bool result = index >= 0;
        if (result)
        {
            RemoveAt(index);
        }
        return result;
    }

    public void RemoveAt(int index)
    {
        T oldItem = objects[index];
        objects.RemoveAt(index);
        AddOperation(Operation.OP_REMOVEAT, index, oldItem, default);
    }

    public T this[int i]
    {
        get => objects[i];
        set
        {
            if (!comparer.Equals(objects[i], value))
            {
                T oldItem = objects[i];
                objects[i] = value;
                AddOperation(Operation.OP_SET, i, oldItem, value);
            }
        }
    }

    public Enumerator GetEnumerator() => new Enumerator(this);

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => new Enumerator(this);

    IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);

    public struct Enumerator : IEnumerator<T>
    {
        readonly SyncList<T> list;
        int index;
        public T Current { get; private set; }

        public Enumerator(SyncList<T> list)
        {
            this.list = list;
            index = -1;
            Current = default;
        }

        public bool MoveNext()
        {
            if (++index >= list.Count)
            {
                return false;
            }
            Current = list[index];
            return true;
        }

        public void Reset() => index = -1;
        object IEnumerator.Current => Current;
        public void Dispose() { }
    }
}