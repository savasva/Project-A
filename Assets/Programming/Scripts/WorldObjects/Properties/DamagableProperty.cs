using System.Collections.Generic;

public class DamagableProperty : WorldObjProperty
{
    public List<WorldObjComponent> components;

    public override BaseAction[] PropActions => new BaseAction[] {
        
    };

    public override Goal[] PropGoals => new Goal[]
    {
        new RepairGoal(null, obj)
    };
}