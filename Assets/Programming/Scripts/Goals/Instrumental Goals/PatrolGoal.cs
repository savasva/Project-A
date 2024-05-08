using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class PatrolGoal : Goal
{
    public override GoalTypes GoalType => GoalTypes.Satisfaction;

    /// <summary>
    /// Should be true if the Colonist is NOT tired
    /// </summary>
    public override Condition[] ResultFits
    {
        get {
            List<Condition> conds = new();

            foreach (WorldObject obj in ColonyManager.inst.damagableObjects)
            {
                conds.Add(new Condition((ColonistState colState, WorldObjInfo objInfo) => -ActionHelpers.Proximity(colState, obj)));
            }

            return conds.ToArray();
        }
    }

    public override bool Evaluate(ColonistState state)
    {
        return true;
    }

    public PatrolGoal() : base() { }

    public PatrolGoal(Colonist _colonist) : base("Patrol.", _colonist) { }
}
