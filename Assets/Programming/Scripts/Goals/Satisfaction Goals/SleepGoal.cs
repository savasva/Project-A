using UnityEngine;
using System.Collections;
using System;
using Cysharp.Threading.Tasks;

[System.Serializable]
public class SleepGoal : Goal
{
    WorldObject bed;
    Needs benefit = new Needs(0, 0, -0.2f, 0);

    /*protected override ConditionSet preconditions
    {
        get
        {
            return new ConditionSet(new Condition(new Needs(-10f, -10f, 0.8f), Condition.Comparison.Above));
        }
    }*/

    /// <summary>
    /// Should be true if the Colonist IS tired
    /// </summary>
    public override Func<ColonistState, float> activationFit {
        get => (ColonistState state) => state.needs.tiredness;
    }

    /// <summary>
    /// Should be true if the Colonist is NOT tired
    /// </summary>
    public override Func<ColonistState, float> resultFit {
        get => (ColonistState state) => -state.needs.tiredness;
    }

    public override bool Evaluate(ColonistState state)
    {
        return state.needs.tiredness >= 0.75f;
    }

    public SleepGoal() : base()
    {
        type = GoalTypes.Satisfaction;
    }

    public SleepGoal(Colonist _colonist, bool _subgoal, Goal _owner = null) : base("Get some sleep.", _colonist, _subgoal, GoalTypes.Satisfaction, _owner) { }
}
