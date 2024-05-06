using System;

[System.Serializable]
public class SleepGoal : Goal
{
    public override GoalTypes GoalType => GoalTypes.Satisfaction;

    /// <summary>
    /// Should be true if the Colonist is NOT tired
    /// </summary>
    public override Condition[] ResultFits {
        get => new Condition[] {
            new Condition((ColonistState colState, WorldObjInfo objInfo) => (-colState.needs.tiredness) - (ActionHelpers.Proximity(colState, objInfo.state)))
        };
    }

    public override bool Evaluate(ColonistState state)
    {
        return state.needs.tiredness >= 0.75f;
    }

    public SleepGoal() : base() { }

    public SleepGoal(Colonist _colonist) : base("Get some sleep.", _colonist) { }
}
