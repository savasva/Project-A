using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Project A/Consumable")]
public class Consumable : InventoryItem
{
    public float consumeTime;
    public Needs nourishment;
}
