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

    public static (ColonistState, BaseAction, float) GetBestAction(Colonist col, ColonistState comparisonState, Condition cond)
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

            //Debug.LogFormat("Action: {0} - Fit: {1}", action.GetType().Name, chosenAction.Item1);

            if (chosenAction.Item1 > bestAction.Weight)
            {
                bestAction.action = chosenAction.Item2;
                bestAction.SetWeight(chosenAction.Item1);
                bestState = chosenAction.Item3;

                //Debug.LogFormat("<b><color=green>Planner:</color></b> Selected action {0} ({1}).", bestAction.action.name, bestAction.Weight);

            }
        }

        /*
         * Check Role actions
         */
        foreach (BaseAction action in col.state.role.Actions)
        {
            chosenAction = action.PredictFit(cond.predicate, comparisonState);

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
        foreach (WorldObject obj in ColonyManager.inst.worldObjects)
        {
            BaseAction[] actions = obj.Actions;

            foreach (BaseAction action in actions)
            {
                //if (i != 0 && parent.Item2.action.GetType() == action.GetType()) continue;

                //Debug.LogFormat("Testing Action {0}", action);

                if (action == null)
                {
                    Debug.LogErrorFormat("<b>{0}:</b> NULL ACTION! Skipping.", obj.gameObject.name);
                    continue;
                }

                chosenAction = action.PredictFit(cond.predicate, comparisonState, obj.info);

                //Debug.LogFormat("Action: {0} ({1}) - Fit: {2}", action.GetType().Name, obj.info.name, chosenAction.Item1);

                if (chosenAction.Item1 > bestAction.Weight)
                {
                    bestAction.action = chosenAction.Item2;
                    bestAction.SetWeight(chosenAction.Item1);
                    bestState = chosenAction.Item3;
                }
            }
        }

        bestAction.action.doer = col;

        return (bestState, bestAction.action, bestAction.Weight);
    }

    public static void GeneratePlanRecursive(Colonist col, ColonistState comparisonState, Condition condition, Plan currentPlan)
    {
        // Find the best action to satisfy the given condition
        (ColonistState bestState, BaseAction bestAction, float weight) = GetBestAction(col, comparisonState, condition);

        Debug.LogFormat("<b><color=green>Planner:</color></b> Selected {0} ({1}).", bestAction.GetType(), weight);

        // If no action is found, error
        if (bestAction == null)
        {
            throw new NotImplementedException(string.Format("There is no valid path that satisfies the condition {0}", condition.predicate.ToString()));
        }

        currentPlan.stack.AddFirst(bestAction);

        foreach (Condition precondition in bestAction.preconditions)
        {
            float predicateFit = precondition.predicate(bestState, WorldObjInfo.none);
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

        Debug.LogFormat("<b><color=green>Planner:</color></b> Building plan for {0}.", goal.GetType());

        foreach(Condition cond in goal.ResultFits)
        {
            GeneratePlanRecursive(col, col.state, cond, plan);
        }

        return plan;
    }
}
