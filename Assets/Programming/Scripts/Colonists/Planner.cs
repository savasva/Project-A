using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Planner : MonoBehaviour
{
    public static Planner inst;

    static System.Type[] primativeActionTypes = new System.Type[] {
        typeof(INGEST),
        //typeof(MTRANS),
        typeof(PTRANS),
        typeof(TAKE)
    };

    BaseAction[] objectActions;

    void Awake()
    {
        if (inst == null)
            inst = this;
        else if (inst != this)
            Destroy(this);
    }

    public static Plan BuildPlan(Colonist col, Goal goal)
    {
        Plan plan = new Plan();

        PlanNode root = plan.AddNode(col.state);

        (PlanNode, PlanEdge) parent = (root, null);
        Func<ColonistState, float> predicate = goal.resultFit;

        /**
         * Step 1: Find the type that fulfills the postcondition of our goal.
         **/

        int i = 0;
        do
        {
            (PlanNode, PlanEdge, float) bestFit = (null, null, float.MinValue);

            (float, BaseAction, ColonistState) chosenAction = (float.MinValue, null, ColonistState.none);

            /*
             * Check every action primative
             */
            foreach (System.Type actionType in primativeActionTypes)
            {
                //Don't let the same action be taken repeatedly
                if (i != 0 && parent.Item2.action.GetType() == actionType) continue;

                //Initialize action
                BaseAction action = (BaseAction)Activator.CreateInstance(actionType);

                //If first go, run based on goal. Otherwise, do based on parent.
                if (i == 0)
                    chosenAction = action.PredictFit(goal.resultFit, parent.Item1.state);
                else
                    chosenAction = action.PredictFit(parent.Item2.action.precondition, parent.Item1.state);

                PlanNode currNode = plan.AddNode(chosenAction.Item3);
                PlanEdge currEdge = plan.AddEdge(chosenAction.Item2, parent.Item1, currNode, chosenAction.Item1);

                if (chosenAction.Item1 > bestFit.Item3)
                {
                    bestFit = (currNode, currEdge, chosenAction.Item1);
                }
            }

            /*
             * Check any objects in the world for actions
             */
            foreach (WorldObject obj in ColonyManager.inst.worldObjects.objects)
            {
                foreach (BaseAction action in obj.actions)
                {
                    if (i != 0 && parent.Item2.action.GetType() == action.GetType()) continue;

                    //If first go, run based on goal. Otherwise, do based on parent.
                    if (i == 0)
                        chosenAction = action.PredictFit(goal.resultFit, parent.Item1.state);
                    else
                        chosenAction = action.PredictFit(parent.Item2.action.precondition, parent.Item1.state);

                    PlanNode currNode = plan.AddNode(chosenAction.Item3);
                    PlanEdge currEdge = plan.AddEdge(chosenAction.Item2, parent.Item1, currNode, chosenAction.Item1);

                    if (chosenAction.Item1 > bestFit.Item3)
                    {
                        bestFit = (currNode, currEdge, chosenAction.Item1);
                    }
                }
            }

            if (bestFit.Item2 == null)
            {
                Debug.LogErrorFormat("No Action could be found that fulfills {0}", parent.Item2.action.name);
                break;
            }
            bestFit.Item2.action.doer = col;
            parent = (bestFit.Item1, bestFit.Item2);
            plan.stack.AddFirst(bestFit.Item2.action);

            Debug.LogFormat("Selected {0} ({1}) at position {2}.", bestFit.Item2.action.GetType(), bestFit.Item3, i);

            i++;
        } while (parent.Item2.action.precondition(parent.Item1.state) <= 0 && i < 3);

        return plan;
    }
}
