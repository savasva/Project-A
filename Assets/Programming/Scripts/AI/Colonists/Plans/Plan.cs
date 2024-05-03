using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Plan
{
    public DoubleEndedQueue<BaseAction> stack;

    public PlanNode root;
    [SerializeField]
    List<PlanNode> nodes;

    [SerializeField]
    List<PlanEdge> edges;

    public Plan()
    {
        nodes = new();
        edges = new();
        stack = new();
    }

    /*public PlanNode AddNode(ColonistState state)
    {
        PlanNode newNode = new PlanNode(state);
        nodes.Add(newNode);

        if (root == null)
        {
            root = newNode;
        }

        return newNode;
    }

    public PlanEdge AddEdge(BaseAction action, PlanNode v1, PlanNode v2)
    {
        PlanEdge newEdge = new PlanEdge(action, v1, v2, 0);
        edges.Add(newEdge);

        return newEdge;
    }

    public PlanEdge AddEdge(BaseAction action, PlanNode v1, PlanNode v2, float weight)
    {
        PlanEdge newEdge = AddEdge(action, v1, v2);
        newEdge.SetWeight(weight);

        return newEdge;
    }*/
}
