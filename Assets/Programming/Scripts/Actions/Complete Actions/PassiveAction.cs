using UnityEngine;
using System;
using System.Collections;

[System.Serializable]
public class PassiveAction<T> : BaseAction where T : class
{
    public Needs tickBenefit;
    Predicate<T> endCond;
    public T input;
    Func<T> pretick;

    public PassiveAction(string _name, Needs _benefit, Predicate<T> _endCondition, Func<T> _endUpdater) : base(_name)
    {
        tickBenefit = _benefit;
        endCond = _endCondition;
        pretick = _endUpdater;
    }

    public override void PreTick()
    {
        input = pretick();
    }

    public override void OnTick()
    {
        doer.needs += tickBenefit * Time.deltaTime;
        if (endCond(input))
        {
            CompleteTask();
        }
    }
}
