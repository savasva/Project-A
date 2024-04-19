using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableProperty : WorldObjectProperty
{
    [Range(0f, 1f)]
    public float breakProgress;

    public override BaseAction[] PropActions => new BaseAction[] {
        //new RepairAction(obj)
    };

    public override Goal[] PropGoals => new Goal[]
    {
        //new RepairGoal(null, obj)
    };
}