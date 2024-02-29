using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class DKnow : Goal
{
    public WorldObject worldObj;

    public DKnow(Colonist _colonist, WorldObject _worldObj) : base("Acquire knowledge.", _colonist, GoalTypes.Delta) {
        worldObj = _worldObj;
    }
}
