using System.Collections.Generic;
using UnityEngine;

public class DamagableProperty : WorldObjProperty
{
    public List<WorldObjComponent> components;

    const float durabilityLoss = 0.01f;

    public override BaseAction[] PropActions => new BaseAction[] {
        
    };

    public override Goal[] PropGoals => new Goal[]
    {
        new RepairGoal(null, obj)
    };

    public override void OnTick()
    {
        foreach (WorldObjComponent prop in components)
        {
            prop.durability -= durabilityLoss * Time.deltaTime;
        }
    }
}