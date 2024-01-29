using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlanNode
{
    public ColonistState state;

    public PlanNode(ColonistState _state)
    {
        state = _state;
    }
}
