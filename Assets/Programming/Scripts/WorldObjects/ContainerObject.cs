using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ContainerObject : WorldObject
{
    public int capacity;
    [SerializeField]
    private int count;

    public List<Consumable> contents;

    public bool IsEmpty { get { return count <= 0; } }

    public bool HasEnough(int amount) {
        return count > amount;
    }

    public void Consume(int amount)
    {
        count -= amount;
    }

    public void Add(Consumable item)
    {
        if (count < capacity)
            contents.Add(item);
    }
}
