using UnityEngine;

public class FlamableProperty : WorldObjProperty
{
    [Range(0f, 1f)]
    public float burnProgress;

    public override BaseAction[] PropActions => new BaseAction[] {
        new ExtinguishAction(obj)
    };

    public override Goal[] PropGoals => new Goal[]
    {
        new ExtinguishGoal(null, obj)
    };

    public override void OnTick()
    {
        //burnProgress += burnProgress * Time.deltaTime;
    }
}
