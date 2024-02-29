using System.Collections.Generic;
using UnityEngine;

public class WorldItem : MonoBehaviour, IInteractable {
    public InventoryItem item;

    public List<Goal> Goals { get; }

    public BaseAction[] Actions => new BaseAction[0];

    public void Take(Colonist taker)
    {
        taker.state.inventory.Add(item);
    }

    public Vector3 GetDestination()
    {
        return transform.position;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}