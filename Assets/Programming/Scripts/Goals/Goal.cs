using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System;

[System.Serializable]
public class Goal
{
    /**
     * Parameters
     **/
    protected Goal owner;
    public Colonist doer = null;
    public Plan plan;
    /*protected virtual ConditionSet preconditions
    {
        get
        {
            return new ConditionSet();
        }
    }*/
    /*public virtual ConditionSet postConditions
    {
        get
        {
            return new ConditionSet();
        }
    }*/

    public virtual Func<ColonistState, float> activationFit {
        get => (ColonistState state) => 1;
    }

    public virtual Func<ColonistState, float> resultFit {
        get => (ColonistState state) => 1;
    }

    public string name;
    public GoalTypes type;
    public GoalState state = GoalState.Queued;
    protected bool subgoal = false;

    /**
     * Actions
     **/
    [SerializeField]
    //protected DoubleEndedQueue<BaseAction> actionHistory = new DoubleEndedQueue<BaseAction>();
    [SerializeReference]
    public List<BaseAction> actionQueueVisualizer = new List<BaseAction>();
    public BaseAction CurrentAction {
        get {
            if (NeedsSubgoal)
                return plan.stack.Peek();
            else
                return CurrentSubgoal.CurrentAction;
        }
    }

    /**
     * Subgoals
     **/
    DoubleEndedQueue<Goal> subgoalQueue = new DoubleEndedQueue<Goal>();
    public List<Goal> subgoalQueueVisualizer = new List<Goal>();
    public Goal CurrentSubgoal { get { return subgoalQueue.Cursor.Value; } }
    public bool NeedsSubgoal { get { return subgoalQueue.Count == 0; } }
    public int SubgoalCount { get { return subgoalQueue.Count; } }

    public Goal() {
        owner = this;
    }

    public Goal(string _name, Colonist _colonist, bool _subgoal, GoalTypes _type, Goal _owner = null)
    {
        name = _name;
        subgoal = _subgoal;
        doer = _colonist;
        type = _type;

        if (_owner == null)
            owner = this;
        else
            owner = _owner;
    }

    public virtual bool Evaluate(ColonistState state)
    {
        //return preconditionFit(col.state);
        return false;
    }

    public async UniTask<bool> Execute(bool interrupt)
    {
        state = GoalState.Started;
        return await Body(interrupt);
    }

    public async virtual UniTask<bool> Body(bool interrupt) { return true; }

    public async virtual UniTask<bool> Do() { return true; }

    public bool CompleteGoal()
    {
        CleanUp();
        state = GoalState.Completed;
        return true;
    }

    public bool FailGoal()
    {
        CleanUp();
        return false;
    }

    public virtual void CleanUp() {
        if (!subgoal)
            doer.CompleteGoal();
        else
            DequeueSubgoal();
        //Debug.Log("Cleaned up!");
    }

    public void SetPlan(Plan newPlan)
    {
        plan = newPlan;
        UpdatePreviews();
    }

    protected void EnqueueAction(BaseAction action)
    {
        if (owner != this)
        {
            owner.EnqueueAction(action);
            return;
        }

        if (action.isInterrupt)
            plan.stack.AddFirst(action);
        else
            plan.stack.Enqueue(action);

        UpdatePreviews();
        Debug.LogFormat("{0}: Enqueued action {1}", GetType(), action.GetType());
    }

    public void InterruptAction(BaseAction action = null)
    {
        if (action.owner != this)
        {
            action.owner.InterruptAction(action);
            return;
        }
        if (CurrentAction != null) CurrentAction.state = BaseAction.ActionState.Interrupted;

        if (action != null) plan.stack.AddFirst(action);

        UpdatePreviews();
    }

    public void CompleteAction()
    {
        if (owner != this)
        {
            owner.CompleteAction();
            return;
        }
        plan.stack.Dequeue();

        if (plan.stack.Count == 0)
            owner.CompleteGoal();

        UpdatePreviews();
    }

    protected void EnqueueSubgoal(Goal subgoal)
    {
        if (owner != this)
        {
            owner.EnqueueSubgoal(subgoal);
            return;
        }

        subgoalQueue.Enqueue(subgoal);
        UpdateCurrentSubgoal();
        UpdatePreviews();
    }

    protected Goal DequeueSubgoal()
    {
        if (owner != this)
        {
            return owner.DequeueSubgoal();
        }
        if (NeedsSubgoal) return null;

        Goal dequeued = subgoalQueue.Dequeue();
        //UpdateCurrentSubgoal();
        UpdatePreviews();
        return dequeued;
    }

    protected void Interrupt(Goal subgoal)
    {
        if (owner != this)
        {
            owner.Interrupt(subgoal);
            return;
        }

        state = GoalState.Interrupted;
        CurrentAction.OnInterrupted();
        subgoalQueue.AddFirst(subgoal);
        UpdateCurrentSubgoal(true);
        Debug.Log(state);
        UpdatePreviews();
    }

    protected void UpdateCurrentSubgoal(bool interrupt = false)
    {
        if (owner != this)
        {
            owner.UpdateCurrentSubgoal(interrupt);
            return;
        }

        if (!NeedsSubgoal && CurrentSubgoal.state != GoalState.Started)
        {
            Debug.LogFormat("{0}: Executing Subgoal {1}", GetType(), CurrentSubgoal.GetType());
            CurrentSubgoal.Execute(interrupt);
        }

        UpdatePreviews();
    }

    protected void CompleteSubgoal()
    {
        if (owner != this)
        {
            owner.CompleteSubgoal();
            return;
        }
        subgoalQueue.Dequeue();
        UpdateCurrentSubgoal();
        UpdatePreviews();
    }

    void UpdatePreviews()
    {
        actionQueueVisualizer = plan.stack.ToList();
        subgoalQueueVisualizer = subgoalQueue.ToList();
    }

    public enum GoalTypes
    {
        Crisis = -1,
        Aspiration = 0,
        Satisfaction = 1,
        Preservation = 2,
        Enjoyment = 2,
        Instrumental = 5,
        Delta = 9
    }

    public enum GoalState
    {
        Queued,
        Interrupted,
        Started,
        Completed
    }
}
