using System;

[Serializable]
public class EatGoal : Goal
{
    public override GoalTypes type => GoalTypes.Satisfaction;

    public override Func<ColonistState, float> activationFit
    {
        get => (ColonistState state) => state.needs.hunger;
    }

    public override Func<ColonistState, WorldObjectInfo, float> resultFit
    {
        get => (ColonistState colState, WorldObjectInfo objInfo) => -colState.needs.hunger;
    }

    public EatGoal() : base() { }

    public EatGoal(Colonist _colonist) : base("Eat.", _colonist) { }
}
