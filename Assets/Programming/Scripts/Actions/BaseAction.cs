using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseAction
{
    public virtual Func<ColonistState, bool> precondition
    {
        get => (ColonistState state) => true;
    }

    public virtual Func<ColonistState, bool> postcondition
    {
        get => (ColonistState state) => true;
    }

    public Goal owner;
    public ActionState state = ActionState.Queued;
    public bool isInterrupt = false;

    public Colonist doer;
    public string name = "Unnamed Task";
    public Needs benefit = new Needs();
    public Action OnComplete = () => { };

    public BaseAction() { }

    public BaseAction(Colonist _doer, string _name, Goal _owner, bool _isInterrupt = false)
    {
        doer = _doer;
        name = _name;
        isInterrupt = _isInterrupt;
        /*if (owner == null)
        {
            owner = doer.CurrentGoal.value;
        }
        else
        {
            owner = _owner;
        }*/
    }

    public virtual void OnStart() {
        state = ActionState.Started;
    }

    public virtual void PreTick() {
        if (state != ActionState.Started) return;
    }

    /**
     * In inherited classes, this should come after anything that updates the completed variable!
     **/
    public virtual void OnTick() {
        if (state != ActionState.Started) return;
    }

    //public abstract bool CheckConditions(ConditionSet[] conditions);

    public virtual void OnInterrupted()
    {
        state = ActionState.Interrupted;
    }

    protected virtual void Complete()
    {
        state = ActionState.Completed;
        OnComplete();
        if (!doer.NeedsGoal)
        {
            doer.CurrentGoal.value.CompleteAction();
        }
    }

    public virtual (float, BaseAction, ColonistState) PredictFit(Goal goal, ColonistState examinee)
    {
        return (0f, null, ColonistState.none);
    }

    public virtual (float, BaseAction, ColonistState) PredictFit(BaseAction prevAction, ColonistState examinee)
    {
        return (0f, null, ColonistState.none);
    }

    public enum ActionState
    {
        Queued,
        Interrupted,
        Started,
        Completed
    }
}
