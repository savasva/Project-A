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
                Debug.LogFormat("{0} -- {1}", name, obj);
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
        name = string.Format("Repair {0}.", obj.info.name);
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

        prop.components[repairIndex].durability += RepairRate * Time.deltaTime;
    }

    protected override void Complete()
    {
        obj.info.state.damaged = false;

        base.Complete();
    }

    public override (float, BaseAction, ColonistState) PredictFit(Func<ColonistState, WorldObjInfo, float> predicate, ColonistState examinee)
    {
        float bestFit = float.MinValue;
        BaseAction bestAction = null;

        foreach (WorldObject currObj in ColonyManager.inst.damagableObjects)
        {
            currObj.info.state.damaged = !currObj.info.state.damaged;

            float fit = predicate(examinee, currObj.info);
            if (fit > bestFit)
            {
                bestAction = new RepairAction(currObj);
                bestFit = fit;
            }

            currObj.info.state.damaged = !currObj.info.state.damaged;
        }

        return (bestFit, bestAction, examinee);
    }
}