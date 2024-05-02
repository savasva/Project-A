using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class DKnow : Goal
{
    public WorldObject worldObj;
    public override GoalTypes GoalType => GoalTypes.Delta;

    public DKnow(Colonist _colonist, WorldObject _worldObj) : base("Acquire knowledge.", _colonist) {
        worldObj = _worldObj;
    }
}
