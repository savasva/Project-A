using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//D-PROX
[System.Serializable]
public class MoveAction : BaseAction
{
    public Vector3 dest;
    public float endDist;
    public bool isObstacle;
    bool hasHandledObstacle = false;
    int framesByObstacle;

    public MoveAction(string _name, Vector3 _dest, float _endDist = 1f) : base(_name) {
        dest = _dest;
        benefit = new Needs(0.01f, 0.01f, 0.01f);
    }

    public override void OnStart()
    {
        doer.mover.stoppingDistance = endDist;
        doer.mover.SetDestination(dest);
        base.OnStart();
    }

    public override void OnTick()
    {
        if (doer.mover.pathPending) return;

        doer.needs += benefit * Time.deltaTime;

        if (doer.mover.remainingDistance <= doer.mover.stoppingDistance)
        {
            doer.mover.ResetPath();
            CompleteTask();
        }
    }

    /**
     * Returns true when Obstacle is properly handled. False otherwise.
     **/
    public bool HandleObstacle(WorldObject obstacle)
    {
        if (obstacle == null) return false;

        if (obstacle.GetType() == typeof(DoorObject))
        {
            doer.currentGoal.value.FailGoal();

            RequestWorldModGoal modGoal = new RequestWorldModGoal(string.Format("CAIN, please open {0}", obstacle.transform.name), obstacle, "Open");
            modGoal.owner = doer;
            modGoal.OnComplete = () =>
            {
                doer.QueueAction(new MoveAction(name, dest));
            };

            doer.Distract(modGoal);
            
            Debug.Log("Obstacle handled!");
            return true;
        }

        return false;
    }
}
