using System;

[Serializable]
public class EatGoal : Goal
{
    public override GoalTypes type => GoalTypes.Satisfaction;

    public override Condition resultFit
    {
        get => new Condition((ColonistState colState, WorldObjectInfo objInfo) => -colState.needs.hunger);
    }

    public override bool Evaluate(ColonistState state)
    {
        return state.needs.hunger >= 0.8f;
    }

    public EatGoal() : base() { }

    public EatGoal(Colonist _colonist) : base("Eat.", _colonist) { }
}
