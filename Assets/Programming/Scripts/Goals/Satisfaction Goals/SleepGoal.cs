using System;

[System.Serializable]
public class SleepGoal : Goal
{
    /// <summary>
    /// Should be true if the Colonist IS tired
    /// </summary>
    public override Func<ColonistState, float> activationFit {
        get => (ColonistState state) => state.needs.tiredness;
    }

    /// <summary>
    /// Should be true if the Colonist is NOT tired
    /// </summary>
    public override Func<ColonistState, float> resultFit {
        get => (ColonistState state) => -state.needs.tiredness;
    }

    public override bool Evaluate(ColonistState state)
    {
        return state.needs.tiredness >= 0.75f;
    }

    public SleepGoal() : base()
    {
        type = GoalTypes.Satisfaction;
    }

    public SleepGoal(Colonist _colonist) : base("Get some sleep.", _colonist, GoalTypes.Satisfaction) { }
}
