using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class Goal
{
    /**
     * Parameters
     **/
    public Colonist doer = null;

    public Plan plan;

    /// <summary>
    /// The criteria that must be met in order for this goal to be valid. The better the match, the higher the value this function should return.
    /// </summary>
    public virtual Condition ResultFit {
        get => new Condition((ColonistState colState, WorldObjInfo objInfo) => 1);
    }

    public GoalState state = GoalState.Queued;

    private string name;
    public virtual GoalTypes GoalType => GoalTypes.Instrumental;
    
    protected bool subgoal = false;

    /**
     * Actions
     **/
    [SerializeField]
    //protected DoubleEndedQueue<BaseAction> actionHistory = new DoubleEndedQueue<BaseAction>();
    [SerializeReference]
    public List<BaseAction> actionQueueVisualizer = new();
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
    public List<Goal> subgoalQueueVisualizer = new();
    public Goal CurrentSubgoal { get { return subgoalQueue.Cursor.Value; } }
    public bool NeedsSubgoal { get { return subgoalQueue.Count == 0; } }
    public int SubgoalCount { get { return subgoalQueue.Count; } }

    public Goal() { }

    public Goal(string _name, Colonist _colonist)
    {
        name = _name;
        doer = _colonist;
    }

    public virtual bool Evaluate(ColonistState state)
    {
        //return preconditionFit(col.state);
        return false;
    }

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
        doer.CompleteGoal();
    }

    public void SetPlan(Plan newPlan)
    {
        plan = newPlan;
        UpdatePreviews();
    }

    protected void EnqueueAction(BaseAction action)
    {
        plan.stack.Enqueue(action);

        UpdatePreviews();
        Debug.LogFormat("{0}: Enqueued action {1}", GetType(), action.GetType());
    }

    public void InterruptAction(BaseAction action = null)
    {
        if (CurrentAction != null) CurrentAction.state = BaseAction.ActionState.Interrupted;

        if (action != null) plan.stack.AddFirst(action);

        UpdatePreviews();
    }

    public void CompleteAction()
    {
        plan.stack.Dequeue();

        if (plan.stack.Count == 0)
            CompleteGoal();

        UpdatePreviews();
    }

    void UpdatePreviews()
    {
        actionQueueVisualizer = plan.stack.ToList();
        subgoalQueueVisualizer = subgoalQueue.ToList();
    }

    public virtual Goal DeepCopy()
    {
        Goal newGoal = (Goal)Activator.CreateInstance(GetType());

        return newGoal;
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
