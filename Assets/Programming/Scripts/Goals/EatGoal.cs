using UnityEngine;
using System.Collections;

public class EatGoal : Goal
{
    [SerializeField]
    ContainerObject foodSource;
    [SerializeField]
    bool atFood = false;
    [SerializeField]
    bool hasFood = false;
    [SerializeField]
    bool eatingFood = false;
    Vector3 consumeLocation;

    public override void OnStart()
    {
        TryToFindFood();
        base.OnStart();
    }

    public override void OnTick()
    {
        if (atFood && !hasFood && foodSource.lineLeader == owner)
        {
            if (!foodSource.IsEmpty)
            {
                foodSource.Consume(1);
                hasFood = true;
            }
            else
            {
                TryToFindFood();
            }
            foodSource.Dequeue();
        }

        if (hasFood && !eatingFood)
        {
            Vector3 eatPos = Random.insideUnitCircle * 5;
            consumeLocation = owner.transform.position + new Vector3(eatPos.x, 0, eatPos.y);

            MoveAction goEat = new MoveAction("Move to eat.", consumeLocation);
            IngestAction eat = new IngestAction("Eat food.", new Consumable(5f, new Needs(-1.5f, 0, 0)));

            eat.OnComplete = () =>
            {
                AccomplishGoal();
            };

            owner.QueueAction(goEat);
            owner.QueueAction(eat);
            eatingFood = true;
        }
    }

    void TryToFindFood()
    {
        foodSource = (ContainerObject)ColonyManager.inst.eatObjects.GetFreestObject();
        MoveAction toFood = ColonyManager.BuildMovementAction(owner, foodSource);

        toFood.OnComplete = () =>
        {
            foodSource.Enqueue(owner);
            atFood = true;
        };

        owner.QueueAction(toFood);
    }
}
