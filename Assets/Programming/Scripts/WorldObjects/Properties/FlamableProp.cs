using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlamableProperty : WorldObjectProperty
{
    [Range(0f, 1f)]
    public float burnProgress;

    public override BaseAction[] PropActions => new BaseAction[] {
        new ExtinguishAction(obj)
    };

    public override Goal[] PropGoals => new Goal[]
    {
        new ExtinguishGoal(null, obj)
    };
}
