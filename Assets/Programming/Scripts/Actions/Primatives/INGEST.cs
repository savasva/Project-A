using System;
using UnityEngine;

[System.Serializable]
public class INGEST : BaseAction
{
    Consumable target;
    float timeActive = 0;

    public INGEST(): base() { }

    public INGEST(Colonist _doer, string _name, Consumable _target, Goal _owner = null, bool _isInterrupt = false) : base(_doer, _name, _owner, _isInterrupt)
    {
        target = _target;
    }

    public override void OnTick()
    {
        base.OnTick();
        timeActive += Time.deltaTime;

        if (timeActive > target.consumeTime)
        {
            doer.state.needs += target.nourishment;
            Complete();
        }
    }

    public override (float, BaseAction, ColonistState) PredictFit(Goal goal, ColonistState examinee)
    {
        //examinee.needs += target.nourishment;
        (float, BaseAction, ColonistState) result = (float.MinValue, null, ColonistState.none);

        foreach (Consumable consumable in ColonyManager.inst.consumables)
        {
            examinee.needs += consumable.nourishment;
            float fit = goal.resultFit(examinee);
            if (fit > result.Item1)
            {
                result = (fit, new INGEST(null, string.Format("Consume a {0}", consumable.name), consumable, goal), examinee);
            }
            examinee.needs -= consumable.nourishment;
        }

        return result;
    }
}
