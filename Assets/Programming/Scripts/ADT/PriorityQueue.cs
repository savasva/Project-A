using System;
using System.Collections.Generic;
using System.Linq;

public class PriorityQueue<T>
{
    List<PriorityNode> queue = new();
    public int Count { get { return queue.Count; } }
    //TODO: default(T) may have unexpected consequences for us later. Be careful of using this function! Debug.Log is your friend.
    public PriorityNode First { get { if (Count > 0) return queue[0]; else return null; } }


    public PriorityQueue()
    {

    }

    public void Enqueue(T item, float priority) {
        queue.Add(new PriorityNode(item, priority));
        Reorder();
    }

    public T Dequeue()
    {
        T first = queue[0].value;
        queue.RemoveAt(0);

        return first;
    }

    void Reorder()
    {
        queue = queue.OrderByDescending(node => node.priority).ToList();
    }

    public float GetPriority(T item)
    {
        PriorityNode found = queue.Find(f => f.value.Equals(item));
        if (found == null) return 0;

        return found.priority;
    }

    public List<T> ToList()
    {
        List<T> list = new List<T>();

        if (Count == 0) return list;

        queue.ForEach(n => list.Add(n.value));

        return list;
    }

    public class PriorityNode
    {
        public float priority;
        public T value;

        public PriorityNode(T _value, float _priority) {
            priority = _priority;
            value = _value;
        }
    }
}
