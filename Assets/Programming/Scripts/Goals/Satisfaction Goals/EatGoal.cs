using System;

[Serializable]
public class EatGoal : Goal
{
    public override GoalTypes type => GoalTypes.Satisfaction;

    public override Condition resultFit
    {
        get => new Condition((ColonistState colState, WorldObjectInfo objInfo) => -colState.needs.hunger);
    }

    public EatGoal() : base() { }

    public EatGoal(Colonist _colonist) : base("Eat.", _colonist) { }
}
