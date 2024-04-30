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
        info.InitProperties(this);
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

    public List<Goal> Goals
    {
        get
        {
            List<Goal> goals = new List<Goal>();

            foreach(WorldObjectProperty prop in info.properties)
            {
                goals.AddRange(prop.PropGoals);
            }

            return goals;
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
public struct WorldObjectInfo
{
    public string name;
    [SerializeReference]
    public List<WorldObjectProperty> properties;
    public WorldObjectState state;

    public WorldObjectInfo(bool isNone = true)
    {
        name = "";
        properties = new List<WorldObjectProperty>();
        state = new WorldObjectState(isNone);
    }

    public static WorldObjectInfo none = new WorldObjectInfo(true);

    public WorldObjectProperty GetProperty(Type property)
    {
        return properties.Find(p => p.GetType() == property);
    }

    public void InitProperties(WorldObject obj)
    {
        foreach (WorldObjectProperty prop in properties)
        {
            prop.InitProperty(obj);
            Debug.Log(prop.PropGoals[0].GetType());
        }
    }

    public List<BaseAction> GetPropertyActions()
    {
        List<BaseAction> final = new List<BaseAction>();

        foreach (WorldObjectProperty prop in properties)
        {
            final.AddRange(prop.PropActions);
        }

        return final;
    }
}

[System.Serializable]
public struct WorldObjectState
{
    public bool isNone;
    public bool aflame;

    public WorldObjectState(bool _none = true)
    {
        aflame = false;
        isNone = _none;
    }

    public static WorldObjectState none
    {
        get {
            return new WorldObjectState();
        }
    }
}