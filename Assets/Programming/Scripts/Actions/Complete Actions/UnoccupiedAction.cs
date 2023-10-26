using UnityEngine;
using System.Collections;

/*
 * Handles scheduling the next added task to the Colonist's queue after it's been depleated.
 */
public class UnoccupiedAction : BaseAction
{
    bool shouldWait;

    public UnoccupiedAction(Colonist _doer, bool _waiting = false) : base("Unoccupied")
    {
        doer = _doer;
        benefit = new Needs(0.01f, 0.01f, 0.005f);
        shouldWait = _waiting;
    }

    public override void OnTick()
    {
        if (shouldWait) return;

        if (doer.NeedsAction)
        {
            doer.ChooseGoal();
        }
        else
        {
            CompleteTask();
        }
    }
}
