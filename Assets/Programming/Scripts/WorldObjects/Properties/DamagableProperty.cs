using System.Collections.Generic;
using UnityEngine;

public class DamagableProperty : WorldObjProperty
{
    public List<WorldObjComponent> components;

    const float DURABILITY_LOSS = 0.001f;

    public override BaseAction[] PropActions => new BaseAction[] {
        
    };

    public override Goal[] PropGoals => new Goal[]
    {
        new RepairGoal(null, obj)
    };

    public override void OnTick()
    {
        DamageTick(DURABILITY_LOSS);
    }

    public void DamageTick(float damage)
    {
        foreach (WorldObjComponent prop in components)
        {
            prop.durability = Mathf.Clamp01(prop.durability - (damage * Time.deltaTime));
        }
    }
}