using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlanEdge
{
    [SerializeField]
    public BaseAction action;
    [SerializeReference]
    PlanNode start;
    [SerializeReference]
    PlanNode end;
    [SerializeField]
    float weight = 0f;

    public PlanEdge(BaseAction _action, PlanNode _start, PlanNode _end, float _weight)
    {
        action = _action;
        start = _start;
        end = _end;
        weight = _weight;
    }

    public void SetStartPoint(PlanNode startNode)
    {
        start = startNode;
    }

    public void SetEndPoint(PlanNode endNode)
    {
        end = endNode;
    }

    public void SetWeight(float newWeight)
    {
        weight = newWeight;
    }
}
