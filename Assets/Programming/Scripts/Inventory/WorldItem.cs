using UnityEngine;

public class WorldItem : MonoBehaviour, IInteractable {
    public InventoryItem item;
    public void Take(Colonist taker)
    {
        taker.state.inventory.Add(item);
        Destroy(gameObject);
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