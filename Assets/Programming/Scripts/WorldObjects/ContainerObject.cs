using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Serialization;
using System;

public class ContainerObject : WorldObject
{
    public Inventory contents;
    public int capacity;

    public bool IsEmpty { get { return contents.Count <= 0; } }

    public bool HasEnough(InventoryItem item, int amount) {
        return contents[item.name].count > amount;
    }

    public bool Consume(InventoryItem item, int amount)
    {
        if (contents[item.name] == null || contents[item.name].count < amount) return false;

        contents[item.name].count -= amount;

        return true;
    }
}
