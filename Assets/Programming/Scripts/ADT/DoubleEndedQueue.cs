using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DoubleEndedQueue<T>
{
    [SerializeField]
    private LinkedList<T> queue = new LinkedList<T>();
    public int Count { get { return queue.Count; } }
    //TODO: default(T) may have unexpected consequences for us later. Be careful of using this function! Debug.Log is your friend.
    public T First { get { if (Count > 0) return queue.First.Value; else return default(T); } }

    public DoubleEndedQueue() { }

    public bool Exists(T value)
    {
        return (queue.Find(value) != null);
    }

    public void Enqueue(T value)
    {
        queue.AddLast(value);
    }

    public T Dequeue()
    {
        if (Count == 0)
        {
            Debug.LogError("Cannot Dequeue from empty list");
            return default(T);
        }

        T firstValue = queue.First.Value;
        queue.RemoveFirst();

        return firstValue;
    }

    public void AddFirst(T value)
    {
        queue.AddFirst(value);
    }

    public T RemoveFirst()
    {
        if (Count == 0) return default(T);

        T temp = queue.First.Value;
        queue.RemoveFirst();

        return temp;
    }

    public List<T> ToList()
    {
        List<T> list = new List<T>();

        if (Count == 0) return list;

        LinkedListNode<T> curr = queue.First;

        do
        {
            list.Add(curr.Value);
            curr = curr.Next;
        }
        while (curr != null);

        return list;
    }
}
