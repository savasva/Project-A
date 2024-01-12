using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObject : MonoBehaviour, IInteractable
{
    public WorldObjectInfo info;
    public Transform moveDestination;
    public Needs benefit;
    
    public bool obstacle = false;
    public bool queueable = false;

    public List<Colonist> queue;
    public Colonist LineLeader { get { if (queue.Count > 0) return queue[0]; return null; } }

    /*
     * Tasks this object is the target of.
     * TODO: If size doesn't change at runtime, this could be an array instead.
     */
    public List<BaseAction> tasks;

    /*
     * Positive biases will encourage a colonist with that trait to interact with this object, and a negative will disuade.
     * TODO: Does this work for people with negative traits? (i.e. Introverts)
     */
    public Big5Personality bias;

    public GameObject room;

    public Colonist owner { get; set; }

    void Start()
    {
        
    }

    /// <summary>
    /// What should this object do when it is interacted with by the Player?
    /// ex: Switch to this camera if it's a CameraObject
    /// </summary>
    public virtual void OnUse()
    {

    }

    public void Enqueue(Colonist col)
    {
        if (queue.IndexOf(col) == -1)
            queue.Add(col);
    }

    public void Dequeue()
    {
        queue.RemoveAt(0);
    }

    public void Withdraw(Colonist col)
    {
        queue.Remove(col);
    }

    public Vector3 GetDestination()
    {
        if (moveDestination != null)
        {
            return moveDestination.position;
        }
        return transform.position;
    }
    public GameObject GetGameObject()
    {
        return gameObject;
    }
}

[System.Serializable]
public struct WorldObjectInfo
{
    public string name;
}