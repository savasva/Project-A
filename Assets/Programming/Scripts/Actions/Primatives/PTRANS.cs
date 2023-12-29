using UnityEngine;

[System.Serializable]
public class PTRANS : BaseAction
{
    Vector3 dest;

    public PTRANS(): base() { }

    public PTRANS(Colonist _doer, string _name, Vector3 _dest, Goal _owner = null, bool _isInterrupt = false) : base(_doer, _name, _owner, _isInterrupt) {
        dest = _dest;
        benefit = new Needs(0.01f, 0.01f, 0.01f);
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

        if (doer.mover.remainingDistance <= doer.mover.stoppingDistance)
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

    public override (float, BaseAction) PredictFit(Goal goal, ColonistState examinee)
    {
        Debug.Log("PTRANS Evaluation");
        (float, BaseAction) result = (0, null);

        foreach (WorldObject obj in ColonyManager.inst.worldObjects.objects)
        {
            examinee.position = obj.GetDestination();
            float fit = goal.postconditionFit(examinee);
            if (fit > result.Item1)
            {
                result = (fit, new PTRANS(null, string.Format("Moving to {0}", dest), dest));
            }
        }

        return result;
    }
}
