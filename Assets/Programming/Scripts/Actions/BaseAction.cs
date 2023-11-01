using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseAction
{
    public bool started;

    public Colonist doer;
    public string name = "Unnamed Task";
    public bool completed = false;
    public Needs benefit = new Needs();
    public Action OnComplete = () => { };

    public BaseAction() { }

    public BaseAction(string _name, BaseAction _caller = null)
    {
        name = _name;
    }

    public virtual void OnStart() {
        started = true;
    }

    public virtual void PreTick() { }

    /**
     * In inherited classes, this should come after anything that updates the completed variable!
     **/
    public virtual void OnTick() { }

    //public abstract bool CheckConditions(ConditionSet[] conditions);

    public virtual void OnInterrupted()
    {

    }

    protected virtual void CompleteTask()
    {
        OnComplete();
        if (doer.NeedsAction)
        {
            Debug.Log(GetType());
            //doer.QueueAction(new UnoccupiedAction(doer));
        }
        else
        {
            doer.DequeueAction();
        }
    }
}
