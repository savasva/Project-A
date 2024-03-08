using System;

[System.Serializable]
public class SleepGoal : Goal
{
    public override GoalTypes type => GoalTypes.Satisfaction;

    /// <summary>
    /// Should be true if the Colonist is NOT tired
    /// </summary>
    public override Condition resultFit {
        get => new Condition((ColonistState colState, WorldObjectInfo objInfo) => -colState.needs.tiredness);
    }

    public override bool Evaluate(ColonistState state)
    {
        return state.needs.tiredness >= 0.75f;
    }

    public SleepGoal() : base() { }

    public SleepGoal(Colonist _colonist) : base("Get some sleep.", _colonist) { }
}
