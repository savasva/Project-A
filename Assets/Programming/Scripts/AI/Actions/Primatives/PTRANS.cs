using System;
using UnityEngine;

[Serializable]
public class PTRANS : BaseAction
{
    Vector3 dest;

    public PTRANS(): base() { }

    public PTRANS(Colonist _doer, string _name, Vector3 _dest) : base(_doer, _name) {
        dest = _dest;
        benefit = new Needs(0.01f, 0.01f, 0.01f, 0.01f, 0);
    }

    public override void OnStart()
    {
        doer.mover.ResetPath();
        doer.mover.SetDestination(dest);
        base.OnStart();
    }

    public override void OnTick()
    {
        base.OnTick();

        doer.state.needs += benefit * Time.deltaTime;

        if (doer.mover.hasPath && doer.mover.remainingDistance <= doer.mover.stoppingDistance)
        {
            doer.mover.ResetPath();
            Complete();
        }
    }

    public override void OnInterrupted()
    {
        base.OnInterrupted();
        doer.mover.ResetPath();
    }

    public override (float, BaseAction, ColonistState) PredictFit(Func<ColonistState, WorldObjInfo, float> predicate, ColonistState examinee)
    {
        (float, BaseAction, ColonistState) result = (float.MinValue, null, ColonistState.none);

        foreach (WorldObject obj in ColonyManager.inst.worldObjects)
        {
            examinee.position = obj.GetDestination();

            float fit = predicate(examinee, WorldObjInfo.none);

            if (fit > result.Item1)
            {
                result = (fit, new PTRANS(null, string.Format("Moving to {0} ({1})", obj.info.name, examinee.position), examinee.position), examinee);
            }
        }

        return result;
    }
}
