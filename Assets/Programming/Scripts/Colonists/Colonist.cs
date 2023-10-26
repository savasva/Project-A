using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Colonist : MonoBehaviour
{
    [Header("\"Who I am\" Variables")]
    public Role role;
    public Big5Personality personality;
    public Needs needs;

    [Header("Scheduling Variables")]
    public int framesPerReevaluation = 30;
    public Dictionary<string, BaseAction> genericTasks;

    DoubleEndedQueue<BaseAction> actionQueue;
    public BaseAction currentAction { get { return actionQueue.First; } }
    public BaseAction lastAction;
    public bool NeedsAction { get { return actionQueue.Count == 0; } }

    PriorityQueue<Goal> goalQueue;
    public PriorityQueue<Goal>.PriorityNode currentGoal { get { return goalQueue.First; } }
    public PriorityQueue<Goal>.PriorityNode lastGoal;
    public bool NeedsGoal { get { return goalQueue.Count == 0; } }

    public List<Goal> goalQueueVisualizer;
    public List<BaseAction> actionQueueVisualizer;

    public NavMeshAgent mover;

    private void Awake()
    {
        mover = GetComponent<NavMeshAgent>();

        genericTasks = new Dictionary<string, BaseAction>();

        actionQueue = new DoubleEndedQueue<BaseAction>();
        goalQueue = new PriorityQueue<Goal>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        OnGameTick();
    }

    /**
     * This function should contain everything that we want to be rechecked every
     * in-game Tick. This is useful incase we want to have things update less frequently than
     * once every frame.
     **/
    void OnGameTick()
    {
        //Update Colonists' state based on the currentTask, as well as progress the Task
        
        if(NeedsGoal)
        {
            ChooseGoal();
            UpdateCurrentGoal();
        }
        else
        {
            if (Time.frameCount % framesPerReevaluation == 0)
            {
                UpdateCurrentGoal();
            }
        }

        if (currentAction != null && !currentAction.started)
        {
            currentAction.OnStart();
        }

        currentAction.PreTick();
        currentAction.OnTick();

        if (currentGoal != null)
            currentGoal.value.OnTick();

        lastGoal = currentGoal;
    }

    public void ChooseGoal()
    {
        Goal goalToQueue;
        float priority;

        if (needs.hunger > 0.75f)
        {
            goalToQueue = new EatGoal();
            priority = 1000f;
        }
        else if (needs.tiredness > 0.75f)
        {
            goalToQueue = new SleepGoal();
            priority = 500f;
        }
        else
        {
            Vector2 ranCirc = Random.insideUnitCircle * 10;
            Vector3 wanderDest = new Vector3(ranCirc.x, transform.position.y, ranCirc.y);

            goalToQueue = new MoveGoal(wanderDest);
            priority = 0f;
        }

        goalToQueue.owner = this;
        goalQueue.Enqueue(goalToQueue, priority);
    }

    public void UpdateCurrentGoal()
    {
        if (currentGoal != null && !currentGoal.value.started)
        {
            currentGoal.value.OnStart();
        }

        goalQueueVisualizer = goalQueue.ToList();
    }

    //Removes top goal from goalQueue. Should be called upon completion.
    public void CompleteGoal()
    {
        goalQueue.Dequeue();
        UpdateCurrentGoal();
    }

    //TODO: Makes a little more sense to make Interruptions their own class of Goal with unique behavior for resuming the last task.
    public void Distract(Goal perscription)
    {
        actionQueue.Dequeue();
        goalQueue.Enqueue(perscription, goalQueue.First.priority + 1);
        UpdateCurrentGoal();
    }

    public void QueueAction(BaseAction task)
    {
        task.doer = this;
        actionQueue.Enqueue(task);

        actionQueueVisualizer = actionQueue.ToList();
    }

    //Removes top action from actionQueue. Should be called upon completion.
    public void CompleteAction()
    {
        actionQueue.Dequeue();
        if (NeedsAction)
            QueueAction(new UnoccupiedAction(this));

        actionQueueVisualizer = actionQueue.ToList();
    }

    public MoveAction BuildMovementAction(WorldObject target)
    {
        Vector3 dir = target.transform.forward;
        Vector3 dest = target.GetDestination() + (dir * target.queue.Count);
        dest.y = transform.position.y;

        MoveAction premovement = new MoveAction(string.Format("Moving to {0}", target.GetGameObject().name), dest);
        premovement.doer = this;
        premovement.dest = dest;

        return premovement;
    }
}