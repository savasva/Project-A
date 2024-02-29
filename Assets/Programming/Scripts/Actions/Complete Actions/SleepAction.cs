using System;
using UnityEngine;

public class SleepAction : BaseAction
{
    [SerializeReference]
    public WorldObject bed;

    public override Func<ColonistState, WorldObjectInfo, float> precondition
    {
        get => (ColonistState colState, WorldObjectInfo objInfo) => {
            return -ActionHelpers.Proximity(colState, bed);
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

    public override (float, BaseAction, ColonistState) PredictFit(Func<ColonistState, WorldObjectInfo, float> predicate, ColonistState examinee)
    {
        //TODO: Update sleep time to be derived from GameTime whenever it is implemented.
        float sleepTime = 200;

        examinee.needs += (benefit * sleepTime);

        return (predicate(examinee, WorldObjectInfo.none), new SleepAction(bed), examinee);
    }
}