using UnityEngine;
using System.Collections;
using Cysharp.Threading.Tasks;

public abstract class Planbox
{
    protected Colonist doer;

    public Planbox(Colonist _doer)
    {
        doer = _doer;
    }

    /*protected static ConditionSet controllablePrecondition;
    protected static ConditionSet uncontrollablePrecondition;
    protected static ConditionSet mediatingPrecondition;

    //TODO: Allow Colonists to pursue correcting the Controllable Precondition
    public static bool Evaluate(ColonistState colState)
    {
        return (controllablePrecondition.IsFulfilled(colState) && uncontrollablePrecondition.IsFulfilled(colState) && mediatingPrecondition.IsFulfilled(colState));
    }*/
}
