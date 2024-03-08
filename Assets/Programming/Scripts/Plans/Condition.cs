using System;
using System.Collections.Generic;
using UnityEngine;

public class Condition
{
    public Func<ColonistState, WorldObjectInfo, float> predicate;

    public Condition(Func<ColonistState, WorldObjectInfo, float> condition)
    {
        predicate = condition;
    }

    public float Evaluate(ColonistState state, WorldObjectInfo objInfo)
    {
        return predicate(state, objInfo);
    }
}
