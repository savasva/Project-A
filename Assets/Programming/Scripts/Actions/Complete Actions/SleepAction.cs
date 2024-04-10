using System;
using UnityEngine;

public class SleepAction : BaseAction
{
    [SerializeReference]
    public WorldObject bed;

    public override Condition[] controllablePreconditions
    {
        get => new Condition[] {
            new Condition((ColonistState colState, WorldObjectInfo objInfo) => {
                return -ActionHelpers.Proximity(colState, bed);
            })
        };
    }

    public override bool[] uncontrollablePreconditions
    {
        get => new bool[]
        {
            bed.queue.Count == 0
        };
    }

    public SleepAction(WorldObject _obj)
    {
        bed = _obj;
    }

    public override void OnStart()
    {
        bed.queue.Add(doer);
        base.OnStart();
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
        bed.queue.Remove(doer);
        base.Complete();
    }

    public override (float, BaseAction, ColonistState) PredictFit(Func<ColonistState, WorldObjectInfo, float> predicate, ColonistState examinee)
    {
        if (!IsValid())
        {
            return (float.MinValue, null, examinee);
        }

        //TODO: Update sleep time to be derived from GameTime whenever it is implemented.
        float sleepTime = 200;

        examinee.needs += (Benefit * sleepTime);

        return (predicate(examinee, WorldObjectInfo.none), new SleepAction(bed), examinee);
    }
}