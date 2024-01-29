using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class DProx : Goal
{
    Vector3 destination;
    WorldObject targetObj;
    WorldObject obstacle = null;

    const float distThresh = 2.5f;

    public override Func<ColonistState, float> activationFit {
        get => (ColonistState state) => Vector3.Distance(state.position, destination);
    }
    public override Func<ColonistState, float> resultFit
    {
        get => (ColonistState state) => -Vector3.Distance(state.position, destination);
    }

    public DProx(Colonist _colonist, bool _subgoal, Vector3 _destination, Goal _owner = null)
        : base(string.Format("Move to {0}", _destination), _colonist, _subgoal, GoalTypes.Delta, _owner) {
        destination = _destination;
    }

    public DProx(Colonist _colonist, bool _subgoal, WorldObject _target, Goal _owner = null)
        : base(string.Format("Move to {0}", _target.name), _colonist, _subgoal, GoalTypes.Delta, _owner)
    {
        targetObj = _target;
    }

    public async override UniTask<bool> Body(bool interrupt)
    {
        PTRANS move = new PTRANS(doer, string.Format("Moving to {0}.", destination.ToString()), destination, owner, interrupt);
        move.doer = doer;
        if (interrupt)
            InterruptAction(move);
        else
            EnqueueAction(move);

        while (move.state != BaseAction.ActionState.Completed)
        {
            if (obstacle == null)
            {
                obstacle = CheckForObstacles();
            }
            
            await UniTask.WaitForEndOfFrame();
        }

        return CompleteGoal();
    }

    WorldObject CheckForObstacles()
    {
        NavMeshHit sample;
        bool hasHit = doer.mover.SamplePathPosition(NavMesh.AllAreas, 3f, out sample);

        if (!hasHit) return null;

        Collider hitCollider = Physics.OverlapBox(sample.position, Vector3.one / 5).FirstOrDefault(f => {
            WorldObject obj = f.GetComponent<WorldObject>();
            if (obj != null && obj.obstacle)
                return true;
            return false;
        });

        if (hitCollider == null) return null;

        WorldObject obst = hitCollider.GetComponent<WorldObject>();

        HandleObstacle(obst);

        return obst;
    }

    void HandleObstacle(WorldObject obstacle)
    {
        Debug.Log(obstacle.name);
        if (obstacle.GetType() == typeof(DoorObject))
        {
            WorldModGoal igoal = new WorldModGoal(doer, true, "CAIN, please open the door.", obstacle, "Open", owner);
            Interrupt(igoal);

            return;
        }

        throw new NotImplementedException(string.Format("The type {0} has no HandleObstacle implementation.", obstacle.GetType()));
    }
}
