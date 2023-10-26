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
    bool atCain = false;
    bool askedForApproval = false;
    int lastLineLength = 0;

    public RequestWorldModGoal(string _requestText, WorldObject _target, string _method)
    {
        requestText = _requestText;
        modTarget = _target;
        modMethod = _method;
    }

    public override void OnStart()
    {
        GoToCain();
        base.OnStart();
    }

    public override void OnTick()
    {
        //If line gets shorter, advance.
        if (atCain && !askedForApproval && lastLineLength > terminal.queue.Count)
        {
            GoToCain();
        }

        //If you're first in line, AskCain!
        if (atCain && !askedForApproval && terminal.lineLeader == owner)
        {
            askedForApproval = true;
            //Ask CAIN to make change.
            CAINManager.inst.AskCain(new CAINManager.CainQuestion(requestText, this));

            //Wait for response.
            owner.QueueAction(new UnoccupiedAction(owner, true));
        }
        lastLineLength = terminal.queue.Count;
    }

    void GoToCain()
    {
        terminal = ColonyManager.inst.cainTerminals.GetNearestObject(owner);
        MoveAction toTerminal = owner.BuildMovementAction(terminal);

        toTerminal.OnComplete = () =>
        {
            terminal.Enqueue(owner);
            atCain = true;
        };

        owner.QueueAction(toTerminal);
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
        owner.CompleteAction();
    }
}
