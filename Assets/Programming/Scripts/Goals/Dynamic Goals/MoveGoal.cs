using UnityEngine;
public class MoveGoal : Goal
{
    [SerializeField]
    Vector3 dest;
    float endDist;

    public MoveGoal(Vector3 _dest, float _endDist = 1f)
    {
        dest = _dest;
        endDist = _endDist;
    }

    public override void OnStart()
    {
        MoveAction move = new MoveAction("Wandering.", dest, endDist);
        move.doer = owner;
        move.OnComplete = () =>
        {
            AccomplishGoal();
        };

        owner.QueueAction(move);
        base.OnStart();
    }
}
