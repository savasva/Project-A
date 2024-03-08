using System;

[Serializable]
public class BaseAction
{
    public virtual Condition[] preconditions
    {
        get => new Condition[0];
    }

    public ActionState state = ActionState.Queued;

    public Colonist doer;
    public string name = "Unnamed Task";
    public Needs benefit = new Needs();
    public Action OnComplete = () => { };

    public BaseAction() { }

    public BaseAction(Colonist _doer, string _name)
    {
        doer = _doer;
        name = _name;
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

    /// <summary>
    /// Determines how well this Action fulfills a given scenario.<br></br>
    /// When chaining actions together we pass in the following action's precondition to ensure that we get to a state where the NPC can accomplish their plan.
    /// </summary>
    /// <param name="predicate">The condition that gives this Action it's weight. Either an Action's precondition or a goal's activationFit.</param>
    /// <param name="examinee">The ColonistState that is being evaluated.</param>
    /// <returns></returns>
    public virtual (float, BaseAction, ColonistState) PredictFit(Func<ColonistState, WorldObjectInfo, float> predicate, ColonistState examinee)
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
