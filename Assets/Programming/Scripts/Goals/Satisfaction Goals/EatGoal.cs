using System;

[Serializable]
public class EatGoal : Goal
{
    public override GoalTypes GoalType => GoalTypes.Satisfaction;

    public override Condition ResultFit
    {
        get => new Condition((ColonistState colState, WorldObjInfo objInfo) => -colState.needs.hunger);
    }

    public EatGoal() : base() { }

    public EatGoal(Colonist _colonist) : base("Eat.", _colonist) { }
}
