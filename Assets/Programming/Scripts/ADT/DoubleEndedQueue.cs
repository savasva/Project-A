using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DoubleEndedQueue<T>
{
    [SerializeField]
    protected LinkedList<T> queue = new LinkedList<T>();
    public int Count { get { return queue.Count - 1; } }
    //TODO: default(T) may have unexpected consequences for us later. Be careful of using this function! Debug.Log is your friend.
    public LinkedListNode<T> Head { get { return queue.First; } }
    LinkedListNode<T> _cursor;
    public LinkedListNode<T> Cursor { get { return _cursor; } }
    public T First { get { if (Count > 0) return Head.Next.Value; else return default(T); } }

    public DoubleEndedQueue() {
        queue.AddFirst(new LinkedListNode<T>(default(T)));
        _cursor = Head;
    }

    public bool Exists(T value)
    {
        return (queue.Find(value) != null);
    }

    public void Enqueue(T value)
    {
        queue.AddLast(value);
        if (Cursor == Head)
            Next();
    }

    public T Dequeue()
    {
        if (Count == 0)
        {
            Debug.LogError("Cannot Dequeue from empty list");
            return default(T);
        }

        if (Cursor == Head.Next)
        {
            if (Cursor.Next == null)
                Prev();
            else
                Next();
        }
            

        T firstValue = Head.Next.Value;
        queue.Remove(Head.Next);

        return firstValue;
    }

    public void AddFirst(T value)
    {
        queue.AddAfter(Head, value);
        if (Cursor == Head)
            Next();
    }

    public LinkedListNode<T> Next()
    {
        if (Cursor.Next == null) return null;

        _cursor = Cursor.Next;

        return _cursor;
    }

    public LinkedListNode<T> Prev()
    {
        if (Cursor == Head || Cursor.Previous == Head) return null;

        _cursor = Cursor.Previous;

        return _cursor;
    }

    public List<T> ToList()
    {
        List<T> list = new List<T>();

        if (Count == 0) return list;

        LinkedListNode<T> curr = Head.Next;

        do
        {
            list.Add(curr.Value);
            curr = curr.Next;
        }
        while (curr != null);

        return list;
    }
}
