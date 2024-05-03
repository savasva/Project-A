using System;
using UnityEngine;

//TODO: Should this be an action tied to the Fire Extinguisher itself??
public class RepairAction : BaseAction
{
    [SerializeField]
    WorldObject obj;
    DamagableProperty prop;
    int repairIndex = 0;
    const float RepairRate = 0.05f;

    public override Condition[] preconditions
    {
        get => new Condition[] {
            new Condition((ColonistState colState, WorldObjInfo objInfo) => {
                return -ActionHelpers.Proximity(colState, obj);
            })
        };
    }

    public RepairAction()
    {

    }

    public RepairAction(WorldObject _obj)
    {
        obj = _obj;
    }

    public override void OnStart()
    {
        base.OnStart();
        prop = obj.info.GetProperty<DamagableProperty>();
    }

    public override void OnTick()
    {
        base.OnTick();

        if (repairIndex == prop.components.Count)
        {
            Complete();
            return;
        }

        if (prop.components[repairIndex].durability >= 1f)
        {
            repairIndex++;
            return;
        }

        prop.components[repairIndex].durability -= RepairRate * Time.deltaTime;
    }

    protected override void Complete()
    {
        obj.info.state.damaged = false;

        base.Complete();
    }

    public override (float, BaseAction, ColonistState) PredictFit(Func<ColonistState, WorldObjInfo, float> predicate, ColonistState examinee)
    {
        foreach (WorldObject currObj in ColonyManager.inst.damagableObjects)
        {
            if (currObj.info.state.damaged)
            {
                return (predicate(examinee, currObj.info), new RepairAction(currObj), examinee);
            }
        }

        return (float.MinValue, null, examinee);
    }
}