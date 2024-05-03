using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FireEmergency : Emergency
{
    public WorldObjectCollection targets;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="intensity">How bad should the emergency be, represented by a float from 0.0 to 1.0.</param>
    public FireEmergency(float intensity)
    {
        intensity = Mathf.Clamp01(intensity);

        List<WorldObject> objs = ColonyManager.inst.flamableObjects.objects;
        int targetCount = Mathf.CeilToInt(objs.Count * intensity);

        //https://stackoverflow.com/questions/48087/select-n-random-elements-from-a-listt-in-c-sharp
        targets = new WorldObjectCollection(objs.OrderBy(o => Random.Range(0f, 1f)).Take(targetCount));
    }

    public FireEmergency(IEnumerable<WorldObject> _targets)
    {
        targets = new WorldObjectCollection(_targets);
    }

    public override void Execute()
    {
        foreach (WorldObject o in targets)
        {
            FlamableProperty flamability = o.info.GetProperty<FlamableProperty>();
            if (flamability == null) continue;

            flamability.burnProgress = Random.Range(0.01f, 0.3f);
            o.info.state.aflame = true;
        }
    }
}

