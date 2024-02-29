using System;
using UnityEngine;

[Serializable]
public class TAKE : BaseAction
{
    WorldItem worldItem;

    //TODO: Precondition based on ownership?
    public override Func<ColonistState, float> precondition
    {
        get => (ColonistState state) =>
        {
            //TODO: Should this be an action tied to the Fire Extinguisher itself??
            return -ActionHelpers.Proximity(state, worldItem);
        };
    }

    public TAKE() : base() { }

    public TAKE(Colonist _doer, string _name, WorldItem _item) : base(_doer, _name)
    {
        worldItem = _item;
        benefit = new Needs(0.01f, 0.01f, 0.01f, 0.01f, 0);
    }

    public override void OnStart()
    {
        worldItem.Take(doer);
        Complete();
        base.OnStart();
    }

    public override (float, BaseAction, ColonistState) PredictFit(Func<ColonistState, float> predicate, ColonistState examinee)
    {
        (float, BaseAction, ColonistState) result = (float.MinValue, null, ColonistState.none);

        foreach (WorldObject obj in ColonyManager.inst.worldObjects.objects)
        {
            examinee.inventory.Add(worldItem.item);

            float fit = predicate(examinee);

            if (fit > result.Item1)
            {
                result = (fit, new PTRANS(null, string.Format("Moving to {0}", examinee.position), examinee.position), examinee);
            }

            examinee.inventory.Remove(worldItem.item);
        }

        return result;
    }
}
