using System;
using UnityEngine;

public class SleepAction : BaseAction
{
    [SerializeReference]
    public WorldObject bed;

    public override Condition[] preconditions
    {
        get => new Condition[] {
            new Condition((ColonistState colState, WorldObjInfo objInfo) => {
                Debug.LogFormat("Distance to {0}: {1}", bed.info.name, ActionHelpers.Proximity(colState, bed));
                return -ActionHelpers.Proximity(colState, bed);
            })
        };
    }

    public SleepAction(WorldObject _obj)
    {
        bed = _obj;
    }

    public override void OnTick()
    {
        base.OnTick();

        doer.state.needs += bed.benefit * Time.deltaTime;

        if (doer.state.needs.tiredness <= -1f)
        {
            Complete();
        }
    }

    protected override void Complete()
    {
        doer.mover.ResetPath();
        base.Complete();
    }

    public override (float, BaseAction, ColonistState) PredictFit(Func<ColonistState, WorldObjInfo, float> predicate, ColonistState examinee)
    {
        //TODO: Update sleep time to be derived from GameTime whenever it is implemented.
        float sleepTime = 200;

        examinee.needs += (benefit * sleepTime);

        return (predicate(examinee, WorldObjInfo.none), new SleepAction(bed), examinee);
    }

    public override (float, BaseAction, ColonistState) PredictFit(Func<ColonistState, WorldObjInfo, float> predicate, ColonistState examinee, WorldObjInfo objInfo)
    {
        //TODO: Update sleep time to be derived from GameTime whenever it is implemented.
        float sleepTime = 200;

        examinee.needs += (benefit * sleepTime);

        return (predicate(examinee, objInfo), new SleepAction(bed), examinee);
    }
}