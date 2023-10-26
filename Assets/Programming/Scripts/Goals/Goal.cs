using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class Goal : ScriptableObject
{
    public bool started;
    public Colonist owner;
    public List<Goal> dependentsGoals;
    public Action OnComplete;

    public virtual void OnStart()
    {
        started = true;
    }

    public virtual void OnTick()
    {
        
    }

    public virtual void AccomplishGoal()
    {
        if (OnComplete != null)
        {
            OnComplete();
        }
        owner.CompleteGoal();
    }

    public virtual void FailGoal()
    {
        
    }

    public virtual void CancelGoal()
    {

    }
}
