using UnityEngine;
using System.Collections;
using System;
using Cysharp.Threading.Tasks;

[System.Serializable]
public class SleepGoal : Goal
{
    WorldObject bed;
    Needs benefit = new Needs(0, 0, -0.2f);

    /*protected override ConditionSet preconditions
    {
        get
        {
            return new ConditionSet(new Condition(new Needs(-10f, -10f, 0.8f), Condition.Comparison.Above));
        }
    }*/

    public override Func<ColonistState, float> preconditionFit {
        get => (ColonistState state) => state.needs.tiredness;
    }
    public override Func<ColonistState, float> postconditionFit {
        get => (ColonistState state) => -state.needs.tiredness;
    }

    public SleepGoal() : base()
    {
        type = GoalTypes.Satisfaction;
    }

    public SleepGoal(Colonist _colonist, bool _subgoal, Goal _owner = null) : base(_colonist, _subgoal, GoalTypes.Satisfaction, _owner) { }

    public async override UniTask<bool> Body(bool interrupt)
    {
        //Go to bed (D-PROX)
        bed = ColonyManager.inst.sleepObjects.GetFreeObject();
        bed.Enqueue(doer);
        DProx dprox = new DProx(doer, true, bed.GetDestination(), this);
        if (!await dprox.Execute(interrupt)) return FailGoal();
        if (!await Do()) return FailGoal();

        return CompleteGoal();
    }

    public async override UniTask<bool> Do()
    {
        while (doer.state.needs.tiredness > -0.5f)
        {
            doer.state.needs += benefit * Time.deltaTime;
            await UniTask.WaitForEndOfFrame();
        }

        return true;
    }

    public override void CleanUp()
    {
        if (bed != null)
            bed.owner = null;

        base.CleanUp();
    }
}
