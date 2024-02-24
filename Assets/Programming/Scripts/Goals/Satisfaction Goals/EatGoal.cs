using UnityEngine;
using System.Collections;
using System;
using Cysharp.Threading.Tasks;

[System.Serializable]
public class EatGoal : Goal
{
    ContainerObject foodContainer;
    Needs benefit = new Needs(0, 0, -0.2f, 0.01f, 0f);

    public override Func<ColonistState, float> activationFit
    {
        get => (ColonistState state) => state.needs.hunger;
    }

    public override Func<ColonistState, float> resultFit
    {
        get => (ColonistState state) => -state.needs.hunger;
    }

    public EatGoal() : base()
    {
        type = GoalTypes.Satisfaction;
    }

    public EatGoal(Colonist _colonist, bool _subgoal, Goal _owner = null) : base("Eat.", _colonist, _subgoal, GoalTypes.Satisfaction, _owner) { }
}
