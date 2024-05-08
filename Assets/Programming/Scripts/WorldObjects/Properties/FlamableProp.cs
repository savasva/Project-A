using UnityEngine;

public class FlamableProperty : WorldObjProperty
{
    [Range(0f, 1f)]
    public float burnProgress = 1;
    DamagableProperty dmgable;

    const float FIRE_DAMAGE = 0.025f;

    public override BaseAction[] PropActions => new BaseAction[] {
        new ExtinguishAction(obj)
    };

    public override Goal[] PropGoals => new Goal[]
    {
        new ExtinguishGoal(null, obj)
    };

    public override void InitProperty(WorldObject owner)
    {
        base.InitProperty(owner);
        if (obj.info.HasProperty<DamagableProperty>())
        {
            dmgable = obj.info.GetProperty<DamagableProperty>();
        }
    }

    public override void OnTick()
    {
        if (!obj.info.state.aflame) return;

        burnProgress = Mathf.Clamp01(burnProgress + (FIRE_DAMAGE * Time.deltaTime));
        if (dmgable != null)
        {
            dmgable.DamageTick(FIRE_DAMAGE);
        }
    }
}
