using System;
using System.Collections.Generic;
using UnityEngine;

public class Condition
{
    public Func<ColonistState, WorldObjInfo, float> predicate;

    public Condition(Func<ColonistState, WorldObjInfo, float> condition)
    {
        predicate = condition;
    }

    /// <summary>
    /// Tests this condition's predicate and returns its results.
    /// </summary>
    /// <param name="state"></param>
    /// <param name="objInfo"></param>
    /// <returns></returns>
    public float Evaluate(ColonistState state, WorldObjInfo objInfo)
    {
        return predicate(state, objInfo);
    }
}
