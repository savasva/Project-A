using UnityEngine;

public class FlamableProperty : WorldObjProperty
{
    [Range(0f, 1f)]
    public float burnProgress = 1;
    DamagableProperty dmgable;

    const float FIRE_DAMAGE = 0.025f;

    [SerializeField]
    ParticleSystem flames;

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

        GameObject flameObj = GameObject.Instantiate(Resources.Load<GameObject>("WorldObjects/Properties/Fire"), obj.gameObject.transform, true);
        flameObj.transform.localPosition = Vector3.zero;

        flames = flameObj.GetComponent<ParticleSystem>();
        flames.Stop();
    }

    public override void OnTick()
    {
        if (!obj.info.state.aflame) { 
            if (flames.isPlaying)
                flames.Stop();

            return;
        };

        if (!flames.isPlaying)
            flames.Play();

        //flames.emission.rateOverTime = 12f * burnProgress;
        var emission = flames.emission;
        var shape = flames.shape;
        var main = flames.main;
        emission.rateOverTime = 12f * burnProgress;
        main.startSize = 0.3f + (0.2f * burnProgress);
        shape.radius = 0.15f + (0.45f * burnProgress);

        burnProgress = Mathf.Clamp01(burnProgress + (FIRE_DAMAGE * Time.deltaTime));
        if (dmgable != null)
        {
            dmgable.DamageTick(FIRE_DAMAGE);
        }
    }
}
