using System;
using UnityEngine;

[Serializable]
public class RepairGoal : Goal
{
    public WorldObject obj;

    public override GoalTypes GoalType => GoalTypes.Preservation;

    /// <summary>
    /// Should be positive if the object is NOT damaged
    /// </summary>
    public override Condition[] ResultFits
    {
        get => new Condition[] {
            new Condition((ColonistState colState, WorldObjInfo objInfo) => (objInfo.HasProperty<DamagableProperty>() && !objInfo.state.isNone && !objInfo.state.damaged) ? 1f : -1f)
        };
    }

    public override bool Evaluate(ColonistState state)
    {
        return obj.info.state.damaged && !obj.info.state.broken;
    }

    public override Goal DeepCopy()
    {
        RepairGoal goal = new RepairGoal(doer, obj);
        return goal;
    }

    public RepairGoal() : base() { }

    public RepairGoal(Colonist _colonist, WorldObject _obj) : base(string.Format("Repair {0}.", _obj.name), _colonist)
    {
        obj = _obj;
    }
}
