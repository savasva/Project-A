using UnityEngine;
using System.Collections;
using System;
using Cysharp.Threading.Tasks;

[System.Serializable]
public class EatGoal : Goal
{
    ContainerObject foodContainer;
    Needs benefit = new Needs(0, 0, -0.2f);

    public override Func<ColonistState, float> preconditionFit
    {
        get => (ColonistState state) => state.needs.hunger;
    }
    public override Func<ColonistState, float> postconditionFit
    {
        get => (ColonistState state) => state.needs.hunger;
    }

    public EatGoal() : base()
    {
        type = GoalTypes.Satisfaction;
    }

    public EatGoal(Colonist _colonist, bool _subgoal, Goal _owner = null) : base(_colonist, _subgoal, GoalTypes.Satisfaction, _owner) { }

    public async override UniTask<bool> Body(bool interrupt)
    {
        //Go to food container (D-PROX)
        foodContainer = (ContainerObject)ColonyManager.inst.eatObjects.GetFreeObject();
        foodContainer.Enqueue(doer);
        DProx dprox = new DProx(doer, true, foodContainer.GetDestination(), this);
        if (!await dprox.Execute(interrupt)) return FailGoal();
        if (!await Do()) return FailGoal();

        return CompleteGoal();
    }

    public async override UniTask<bool> Do()
    {
        INGEST ingest = new INGEST(doer, string.Format("Consuming {0}."), new Consumable(5f, new Needs(5f, 0f, 0f)), this);
        while (ingest.state != BaseAction.ActionState.Completed)
        {
            await UniTask.WaitForEndOfFrame();
        }

        return true;
    }

    public override void CleanUp()
    {
        if (foodContainer != null)
            foodContainer.owner = null;

        base.CleanUp();
    }
}
