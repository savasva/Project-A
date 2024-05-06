using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Should this be an action tied to the Fire Extinguisher itself??
public class ExtinguishAction : BaseAction
{
    [SerializeField]
    WorldObject obj;
    FlamableProperty prop;
    const float ExtinguishRate = 0.05f;

    public override Condition[] preconditions
    {
        get => new Condition[] {
            new Condition((ColonistState colState, WorldObjInfo objInfo) => {
                return -ActionHelpers.Proximity(colState, obj);
            }),
            new Condition((ColonistState colState, WorldObjInfo objInfo) => {
                return colState.inventory.Has("Fire Extinguisher") ? 1 : -1;
            })
            
        };
    }

    public ExtinguishAction(WorldObject _obj)
    {
        obj = _obj;
    }

    public override void OnStart()
    {
        base.OnStart();
        prop = obj.info.GetProperty<FlamableProperty>();
        Debug.Log(obj.name);
        Debug.Log(prop.burnProgress);
    }

    public override void OnTick()
    {
        base.OnTick();

        if (prop.burnProgress <= 0)
        {
            Complete();
        }

        prop.burnProgress -= ExtinguishRate * Time.deltaTime;
    }

    protected override void Complete()
    {
        obj.info.state.aflame = false;

        base.Complete();
    }

    public override (float, BaseAction, ColonistState) PredictFit(Func<ColonistState, WorldObjInfo, float> predicate, ColonistState examinee)
    {
        if (obj.info.state.aflame)
        {
            obj.info.state.aflame = false;
            float fit = predicate(examinee, obj.info);
            obj.info.state.aflame = true;
            return (fit, new ExtinguishAction(obj), examinee);
        }

        return (float.MinValue, null, examinee);
    }
}