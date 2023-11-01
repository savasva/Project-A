using UnityEngine;
using System.Collections;
using System;

public class SleepGoal : Goal
{
    [SerializeField]
    WorldObject bed;
    [SerializeField]
    bool inBed = false;
    [SerializeField]
    bool sleeping = false;

    public override void OnStart()
    {
        GoToBed();
        base.OnStart();
    }

    public override void OnTick()
    {
        if (inBed && !sleeping)
        {
            PassiveAction<Needs> sleep = new PassiveAction<Needs>("Sleeping.", new Needs(0, 0, -0.2f),
                new Predicate<Needs>(delegate (Needs n) { return n.tiredness < -0.5f; }),
                () => { return owner.needs; });

            sleep.OnComplete = () =>
            {
                CleanUp();
                AccomplishGoal();
            };

            owner.QueueAction(sleep);
            sleeping = true;
        }
    }

    void GoToBed()
    {
        bed = ColonyManager.inst.sleepObjects.GetFreeObject();

        if (bed == null)
        {
            CancelGoal();
            return;
        }

        MoveAction toBed =  ColonyManager.BuildMovementAction(owner, bed);

        bed.Enqueue(owner);
        toBed.OnComplete = () =>
        {
            inBed = true;
        };

        owner.QueueAction(toBed);
    }

    public override void FailGoal()
    {

    }

    public override void CancelGoal() {
        CleanUp();
        AccomplishGoal();
    }


    void CleanUp()
    {
        if (bed != null)
            bed.Dequeue();
    }
}
