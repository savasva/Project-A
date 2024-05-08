using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class WorldObject : MonoBehaviour, IInteractable
{
    [SerializeReference]
    public BaseAction[] actions;

    public WorldObjInfo info;
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
        info.InitProperties(this);
    }

    void Update()
    {
        info.state.position = transform.position;

        foreach (WorldObjProperty prop in info.properties)
        {
            prop.OnTick();
        }
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

    public BaseAction[] Actions => actions.Concat(info.GetPropertyActions()).ToArray();

    public Goal[] Goals
    {
        get
        {
            List<Goal> goals = new();

            foreach(WorldObjProperty prop in info.properties)
            {
                goals.AddRange(prop.PropGoals);
            }

            return goals.ToArray();
        }
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
public struct WorldObjInfo
{
    public string name;
    [SerializeReference]
    public List<WorldObjProperty> properties;
    public WorldObjState state;

    public WorldObjInfo(bool isNone = true)
    {
        name = "";
        properties = new List<WorldObjProperty>();
        state = new WorldObjState(isNone);
    }

    public static WorldObjInfo none = new WorldObjInfo(true);

    public T GetProperty<T>() where T : WorldObjProperty
    {
        return (T)properties.Find(p => p.GetType() == typeof(T));
    }

    public bool HasProperty<T>() where T : WorldObjProperty
    {
        return (T)properties.Find(p => p.GetType() == typeof(T)) != null;
    }

    public void InitProperties(WorldObject obj)
    {
        state.position = obj.transform.position;
        foreach (WorldObjProperty prop in properties)
        {
            prop.InitProperty(obj);
        }
    }

    public List<BaseAction> GetPropertyActions()
    {
        List<BaseAction> final = new();

        foreach (WorldObjProperty prop in properties)
        {
            final.AddRange(prop.PropActions);
        }

        return final;
    }
}

[System.Serializable]
public struct WorldObjState
{
    public Vector3 position;
    public bool isNone;
    public bool aflame;
    public bool damaged;
    public bool broken;

    public WorldObjState(bool _none = true)
    {
        position = Vector3.zero;
        aflame = false;
        damaged = false;
        broken = false;
        isNone = _none;
    }

    public static WorldObjState none
    {
        get {
            return new WorldObjState();
        }
    }
}