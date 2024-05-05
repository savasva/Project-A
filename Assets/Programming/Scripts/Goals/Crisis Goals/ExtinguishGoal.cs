using System;
using UnityEngine;

[Serializable]
public class ExtinguishGoal : Goal
{
    public WorldObject obj;

    public override GoalTypes GoalType => GoalTypes.Crisis;

    /// <summary>
    /// Should be positive if the object is NOT on fire
    /// </summary>
    public override Condition[] ResultFits
    {
        get => new Condition[] {
            new Condition((ColonistState colState, WorldObjInfo objInfo) =>  (!objInfo.state.isNone && !objInfo.state.aflame) ? 1f : -1f)
        };
    }

    public override bool Evaluate(ColonistState state)
    {
        return obj.info.state.aflame;
    }

    public override Goal DeepCopy()
    {
        ExtinguishGoal goal = new ExtinguishGoal(doer, obj);
        return goal;
    }

    public ExtinguishGoal() : base() { }

    public ExtinguishGoal(Colonist _colonist, WorldObject _obj) : base(string.Format("Extinguish {0}.", _obj.name), _colonist) {
        obj = _obj;
    }
}
