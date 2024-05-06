using System;

[Serializable]
public class EatGoal : Goal
{
    public override GoalTypes GoalType => GoalTypes.Satisfaction;

    public override Condition[] ResultFits
    {
        get => new Condition[] {
            new Condition((ColonistState colState, WorldObjInfo objInfo) => -colState.needs.hunger)
        };
    }

    public EatGoal() : base() { }

    public EatGoal(Colonist _colonist) : base("Eat.", _colonist) { }
}
