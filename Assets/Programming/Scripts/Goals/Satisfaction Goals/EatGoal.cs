using System;

[Serializable]
public class EatGoal : Goal
{
    public override Func<ColonistState, float> activationFit
    {
        get => (ColonistState state) => state.needs.hunger;
    }

    public override Func<ColonistState, float> resultFit
    {
        get => (ColonistState state) => -state.needs.hunger;
    }

    public EatGoal() : base()
    {
        type = GoalTypes.Satisfaction;
    }

    public EatGoal(Colonist _colonist) : base("Eat.", _colonist, GoalTypes.Satisfaction) { }
}
