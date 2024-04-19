using System;
using UnityEngine;

[Serializable]
public class TAKE : BaseAction
{
    WorldItem worldItem;
    bool destroyOnComplete = false;

    //TODO: Precondition based on ownership?
    public override Condition[] preconditions
    {
        get => new Condition[] {
            new Condition((ColonistState colState, WorldObjectInfo objInfo) =>
            {
                //TODO: Should this be an action tied to the Fire Extinguisher itself??
                return -ActionHelpers.Proximity(colState, worldItem);
            })
        };
    }

    public TAKE() : base() { }

    public TAKE(Colonist _doer, string _name, WorldItem _item, bool _destroyOnComplete = false) : base(_doer, _name)
    {
        worldItem = _item;
        benefit = new Needs(0.01f, 0.01f, 0.01f, 0.01f, 0);
        destroyOnComplete = _destroyOnComplete;
    }

    public override void OnStart()
    {
        worldItem.Take(doer);
        base.OnStart();
    }

    public override void OnTick()
    {
        Complete();
    }

    public override (float, BaseAction, ColonistState) PredictFit(Func<ColonistState, WorldObjectInfo, float> predicate, ColonistState examinee)
    {
        (float, BaseAction, ColonistState) result = (float.MinValue, null, ColonistState.none);

        foreach (WorldItem worldItem in ColonyManager.inst.worldItems)
        {
            examinee.inventory.Add(worldItem.item);

            float fit = predicate(examinee, WorldObjectInfo.none);

            if (fit > result.Item1)
            {
                result = (fit, new TAKE(null, string.Format("Taking {0}.", worldItem.name), worldItem), examinee);
                Debug.Log(worldItem.name);
            }

            examinee.inventory.Remove(worldItem.item);
        }

        return result;
    }
}
