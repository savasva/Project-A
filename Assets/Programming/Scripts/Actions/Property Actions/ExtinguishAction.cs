using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtinguishAction : BaseAction
{
    [SerializeField]
    WorldObject obj;
    FlamableProperty prop;
    const float ExtinguishRate = 0.05f;

    public override Func<ColonistState, float> precondition
    {
        get => (ColonistState state) => {
            //TODO: Should this be an action tied to the Fire Extinguisher itself??
            return (obj.state.aflame && state.inventory.Has("Fire Extinguisher")) ? 1 : -1;
        };
    }

    public ExtinguishAction(WorldObject _obj)
    {
        obj = _obj;
    }

    public override void OnStart()
    {
        base.OnStart();
        prop = (FlamableProperty)obj.info.GetProperty(typeof(FlamableProperty));
    }

    public override void OnTick()
    {
        base.OnTick();

        if (prop.burnProgress == 0)
        {
            Complete();
        }

        prop.burnProgress -= ExtinguishRate * Time.deltaTime;
    }

    protected override void Complete()
    {
        obj.state.aflame = false;

        base.Complete();
    }

    public override (float, BaseAction, ColonistState) PredictFit(Func<ColonistState, float> predicate, ColonistState examinee)
    {
        return (predicate(examinee), new ExtinguishAction(obj), examinee);
    }
}