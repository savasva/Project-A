using System;
using UnityEngine;

//TODO: This should probably be abstracted into a generalized "AskCain" goal.
public class RequestWorldModGoal : Goal
{
    WorldObject terminal;
    string requestText;
    WorldObject modTarget;
    //This is a method to be run on the given modTarget. Will be executed with Broadcast.
    string modMethod;
    [SerializeField]
    bool atCain = false;
    [SerializeField]
    bool askedForApproval = false;
    int repathRate = 60;

    public RequestWorldModGoal(string _requestText, WorldObject _target, string _method)
    {
        requestText = _requestText;
        modTarget = _target;
        modMethod = _method;
    }

    public override void OnStart()
    {
        owner.QueueAction(BuildCainMovement());
        owner.QueueAction(new UnoccupiedAction(owner, true));
        base.OnStart();
    }

    public override void OnTick()
    {
        //If not at terminal yet, repeatedly repath to maintain proper place in line.
        if (!atCain && Time.frameCount % repathRate == 0)
        {
            MoveAction nextMove;

            if (owner.currentAction.GetType() == typeof(MoveAction))
            {
                owner.DequeueAction();
            }

            if (terminal.queue.Count > 0 && terminal.LineLeader != owner)
            {
                //Don't allow to prompt before reaching front of line
                atCain = false;

                //Move to next position in line
                MoveAction lineAdvance = ColonyManager.BuildMovementAction(owner, terminal);
                owner.InterruptAction(lineAdvance);
            }
            else if (terminal.LineLeader == owner)
            {
                owner.InterruptAction(BuildCainMovement());
            }
        }

        //If you're first in line, AskCain!
        if (atCain && !askedForApproval && terminal.LineLeader == owner)
        {
            askedForApproval = true;
            //Ask CAIN to make change.
            CAINManager.inst.AskCain(new CAINManager.CainQuestion(requestText, this));
        }
    }

    MoveAction BuildCainMovement()
    {
        terminal = ColonyManager.inst.cainTerminals.GetNearestObject(owner);
        MoveAction toTerminal = ColonyManager.BuildMovementAction(owner, terminal);
        terminal.Enqueue(owner);

        toTerminal.OnComplete = () =>
        {
            atCain = true;
        };

        //Move to Terminal and then wait.
        return toTerminal;
    }

    public void ReceiveResponse(bool approved)
    {
        if (approved)
        {
            modTarget.BroadcastMessage(modMethod);
            AccomplishGoal();
        }
        else
        {
            FailGoal();
        }

        //Leave line for terminal.
        terminal.Dequeue();
        //Stop waiting.
        owner.currentAction.OnComplete();
        owner.DequeueAction();
    }
}
