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

    public override Func<ColonistState, WorldObjectInfo, float> precondition
    {
        get => (ColonistState colState, WorldObjectInfo objInfo) => {
            //TODO: Should this be an action tied to the Fire Extinguisher itself??
            return (colState.inventory.Has("Fire Extinguisher")) ? 1 : -1;
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

    public override (float, BaseAction, ColonistState) PredictFit(Func<ColonistState, WorldObjectInfo, float> predicate, ColonistState examinee)
    {
        /*foreach (WorldObject currObj in ColonyManager.inst.flamableObjects.objects)
        {
            Debug.Log(currObj.info.state.aflame);
            if (currObj.info.state.aflame)
            {
                return (predicate(examinee, currObj.info), new ExtinguishAction(currObj), examinee);
            }
        }*/

        Debug.LogFormat("{0}, {1}", obj.info.state.aflame, predicate(examinee, obj.info));
        if (obj.info.state.aflame)
        {
            obj.info.state.aflame = false;
            return (predicate(examinee, obj.info), new ExtinguishAction(obj), examinee);
        }

        return (float.MinValue, null, examinee);
    }
}