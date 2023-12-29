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
        typeof(PTRANS)
    };

    static BaseAction[] objectActions;

    void Awake()
    {
        if (inst == null)
            inst = this;
        else if (inst != this)
            Destroy(this);
    }

    public static void BuildPlan(Colonist col, Goal goal)
    {
        /**
         * Step 1: Find the type that fulfills the postcondition of our goal.
         **/
        (float, BaseAction) bestFit = (0, null);
        foreach (System.Type actionType in primativeActionTypes)
        {
            BaseAction action = (BaseAction)Activator.CreateInstance(actionType);
            //Debug.LogFormat("Before: {0}", col.state.ToString());

            (float, BaseAction) chosenAction = action.PredictFit(goal, col.state);

            Debug.Log(chosenAction);
            if (chosenAction.Item1 > bestFit.Item1)
            {
                Debug.Log(actionType.Name);
            }

            //Debug.LogFormat("After: {0}", col.state.ToString());
        }
    }
}
