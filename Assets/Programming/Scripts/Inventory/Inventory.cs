using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory : SerializedDictionary<string, InventorySlot>
{
    public new int Count
    {
        get
        {
            int count = 0;

            foreach (InventorySlot slot in this.Values)
            {
                count += slot.count;
            }

            return count;
        }
    }

    public void Add(InventoryItem item)
    {
        if (!ContainsKey(item.name))
        {
            Add(item.name, new InventorySlot(item));
        }
        else
        {
            this[item.name].count++;
        }
    }

    public void Remove(InventoryItem item)
    {
        if (!ContainsKey(item.name) || this[item.name].count == 0) return;
        
        this[item.name].count--;
    }
}