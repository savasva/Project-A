using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public InventoryItem item;
    public int count;

    public InventorySlot(InventoryItem _item)
    {
        item = _item;
        count = 1;
    }
}
