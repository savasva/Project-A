using System;
using UnityEngine;

[Serializable]
public class ExtinguishGoal : Goal
{
    public WorldObject obj;

    public override GoalTypes type => GoalTypes.Crisis;

    /// <summary>
    /// Should be true if object is on fire
    /// </summary>
    public override Func<ColonistState, float> activationFit
    {
        get => (ColonistState state) => obj.info.state.aflame ? 1f : -1f;
    }

    /// <summary>
    /// Should be true if the object is NOT on fire
    /// </summary>
    public override Func<ColonistState, WorldObjectInfo, float> resultFit
    {
        get => (ColonistState colState, WorldObjectInfo objInfo) =>  (!objInfo.state.isNone && !objInfo.state.aflame) ? 1f : -1f;
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
