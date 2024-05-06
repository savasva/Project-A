using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class DProx : Goal
{
    public override GoalTypes GoalType => GoalTypes.Delta;

    Vector3 destination;

    public override Condition[] ResultFits
    {
        get => new Condition[] {
            new Condition((ColonistState colState, WorldObjInfo objInfo) => -Vector3.Distance(colState.position, destination))
        };
    }

    public DProx(Colonist _colonist, Vector3 _destination)
        : base(string.Format("Move to {0}", _destination), _colonist) {
        destination = _destination;
    }

    public DProx(Colonist _colonist, WorldObject _target)
        : base(string.Format("Move to {0}", _target.name), _colonist)
    {
        destination = _target.GetDestination();
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
        if (obstacle.GetType() == typeof(DoorObj))
        {
            WorldModGoal igoal = new WorldModGoal(doer, "CAIN, please open the door.", obstacle, "Open");
            //Interrupt(igoal);

            //return;
        }

        throw new NotImplementedException(string.Format("The type {0} has no HandleObstacle implementation.", obstacle.GetType()));
    }
}
