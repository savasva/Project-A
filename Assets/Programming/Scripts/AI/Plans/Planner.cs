using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Planner : MonoBehaviour
{
    public static Planner inst;

    public const int maxPlanSteps = 3;

    static BaseAction[] PrimativeActions = new BaseAction[] {
        new INGEST(),
        new PTRANS(),
        new TAKE()
    };

    void Awake()
    {
        if (inst == null)
            inst = this;
        else if (inst != this)
            Destroy(this);
    }

    public static (ColonistState, BaseAction) GetBestAction(Colonist col, ColonistState comparisonState, Condition cond)
    {
        //PlanNode bestAction = new PlanNode(colState);
        //PlanEdge bestEdge = new PlanEdge(null, null, null, float.MinValue);
        PlanEdge bestAction = new PlanEdge(null, null, null, float.MinValue);
        ColonistState bestState = comparisonState;

        (float, BaseAction, ColonistState) chosenAction = (float.MinValue, null, ColonistState.none);

        /*
         * Check every action primative
         */
        foreach (BaseAction action in PrimativeActions)
        {
            //Don't let the same action be taken repeatedly
            //if (parent.Item2.action.GetType() == actionType) continue;

            chosenAction = action.PredictFit(cond.predicate, comparisonState);

            Debug.LogFormat("Action: {0} - Fit: {1}", action.GetType().Name, chosenAction.Item1);

            if (chosenAction.Item1 > bestAction.Weight)
            {
                bestAction.action = chosenAction.Item2;
                bestAction.SetWeight(chosenAction.Item1);
                bestState = chosenAction.Item3;
            }
        }

        /*
         * Check any objects in the world for actions
         */
        foreach (WorldObject obj in ColonyManager.inst.worldObjects.objects)
        {
            BaseAction[] actions = obj.Actions;

            foreach (BaseAction action in actions)
            {
                 //if (i != 0 && parent.Item2.action.GetType() == action.GetType()) continue;

                chosenAction = action.PredictFit(cond.predicate, comparisonState);

                Debug.LogFormat("Action: {0} - Fit: {1}", action.GetType().Name, chosenAction.Item1);

                if (chosenAction.Item1 > bestAction.Weight)
                {
                    bestAction.action = chosenAction.Item2;
                    bestAction.SetWeight(chosenAction.Item1);
                    bestState = chosenAction.Item3;
                }
            }
        }

        bestAction.action.doer = col;

        return (bestState, bestAction.action);
    }

    public static void GeneratePlanRecursive(Colonist col, ColonistState comparisonState, Condition condition, Plan currentPlan)
    {
        // Find the best action to satisfy the given condition
        (ColonistState bestState, BaseAction bestAction) = GetBestAction(col, comparisonState, condition);

        Debug.LogFormat("Selected {0}", bestAction.GetType().Name);

        // If no action is found, error
        if (bestAction == null)
        {
            throw new NotImplementedException(string.Format("There is no valid path that satisfies the condition {0}", condition.predicate.ToString()));
        }

        currentPlan.stack.AddFirst(bestAction);

        foreach (Condition precondition in bestAction.preconditions)
        {
            float predicateFit = precondition.predicate(bestState, WorldObjectInfo.none);
            if (predicateFit <= 0)
            {
                // Recursively generate a sub-plan to satisfy the unmet precondition
                GeneratePlanRecursive(col, bestState, precondition, currentPlan);
            }
        }

        return;
    }

    public static Plan BuildPlan(Colonist col, Goal goal)
    {
        Plan plan = new Plan();
        GeneratePlanRecursive(col, col.state, goal.ResultFit, plan);

        return plan;
    }
}
