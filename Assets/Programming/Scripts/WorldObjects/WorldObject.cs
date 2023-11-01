using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Authors Note:
 * I've tried to optimize this script to the point that it's mangled, sorry.
 */
public class WorldObject : MonoBehaviour, IInteractable
{
    public Transform moveDestination;
    public Needs benefit;
    public bool obstacle;
    public List<Colonist> queue;
    public Colonist lineLeader { get { if (queue.Count > 0) return queue[0]; else return null; } }

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
    public Colonist owner;

    public MonoBehaviour taskType;

    void Start()
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
