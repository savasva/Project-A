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
        if (this[item.name] == null)
        {
            Add(item.name, new InventorySlot(item));
        }
        else
        {
            this[item.name].count++;
        }
    }


}