using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepAction : BaseAction
{
    WorldObject bed;

    public SleepAction(WorldObject _sleepObj) : base("Sleep")
    {
        bed = _sleepObj;
    }

    public override void OnTick()
    {
        doer.needs += bed.benefit;
        if (doer.needs.tiredness < 0)
        {
            CompleteTask();
        }
    }
}
