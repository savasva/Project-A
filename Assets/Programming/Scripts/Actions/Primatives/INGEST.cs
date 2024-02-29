using System;
using UnityEngine;

[System.Serializable]
public class INGEST : BaseAction
{
    public override Func<ColonistState, WorldObjectInfo, float> precondition
    {
        get => (ColonistState colState, WorldObjectInfo objInfo) =>
        {
            if (!colState.inventory.ContainsKey(target.name) || colState.inventory[target.name].count == 0) return float.MinValue;

            return colState.inventory[target.name].count;
        };
    }

    public override Func<ColonistState, float> postcondition
    {
        get => (ColonistState state) => Needs.Difference(state.needs, state.needs + target.nourishment);
    }

    Consumable target;
    float timeActive = 0;

    public INGEST(): base() { }

    public INGEST(Colonist _doer, string _name, Consumable _target) : base(_doer, _name)
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

    public override (float, BaseAction, ColonistState) PredictFit(Func<ColonistState, WorldObjectInfo, float> predicate, ColonistState examinee)
    {
        (float, BaseAction, ColonistState) result = (float.MinValue, null, ColonistState.none);

        foreach (Consumable consumable in ColonyManager.inst.consumables)
        {
            examinee.needs += consumable.nourishment;
            float fit = predicate(examinee, WorldObjectInfo.none);
            if (fit > result.Item1)
            {
                result = (fit, new INGEST(null, string.Format("Consume a {0}", consumable.name), consumable), examinee);
            }
            examinee.needs -= consumable.nourishment;
        }

        return result;
    }
}
