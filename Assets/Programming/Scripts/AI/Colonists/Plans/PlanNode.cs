using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlanNode
{
    public BaseAction action;
    public ColonistState state;
    public float weight;

    public PlanNode(BaseAction _action, ColonistState _state, float _weight)
    {
        action = _action;
        state = _state;
        weight = _weight;
    }
}
