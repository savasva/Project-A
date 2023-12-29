using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class DKnow : Goal
{
    public WorldObject worldObj;

    public DKnow(Colonist _colonist, bool _subgoal, WorldObject _worldObj) : base(_colonist, _subgoal, GoalTypes.Delta) {
        worldObj = _worldObj;
    }

    public async override UniTask<bool> Body(bool interrupt)
    {
        //return (knows location of WorldObject)
        return CompleteGoal();
    }

    public async override UniTask<bool> Do() { return true; }
}
