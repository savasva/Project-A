using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Colonist : MonoBehaviour
{
    [Header("\"Who I am\" Variables")]
    public ColonistState state;
    public Big5Personality personality;

    [Header("Memory")]
    //Short-Term Memory / Working Memory
    public MemoryContainer stm;
    //Long-Term Memory
    public MemoryContainer ltm;

    BaseAction CurrentAction {
        get {
            if (NeedsGoal) return null;

            if (CurrentGoal.value.NeedsSubgoal)
                return CurrentGoal.value.CurrentAction;
            else
                return CurrentSubaction;
        }
    }

    BaseAction CurrentSubaction
    {
        get
        {
            if (NeedsGoal) return null;

            return CurrentGoal.value.CurrentSubgoal.CurrentAction;
        }
    }

    PriorityQueue<Goal> goalQueue;
    public PriorityQueue<Goal>.PriorityNode CurrentGoal {
        get {
            return goalQueue.First;
        }
    }
    [SerializeField]
    Goal _currentGoal;

    public bool NeedsGoal {
        get {
            return goalQueue.Count == 0;
        }
    }
    public List<Goal> goalQueueVisualizer;

    [Header("Pathfinding")]
    public NavMeshAgent mover;
    int mobileAvoidance;
    [SerializeField]
    int staticAvoidance;

    List<Type> personalGoalPool;

    void Awake()
    {
        mover = GetComponent<NavMeshAgent>();

        //actionQueue = new DoubleEndedQueue<BaseAction>();
        goalQueue = new PriorityQueue<Goal>();
        mobileAvoidance = mover.avoidancePriority;
        UpdateState();
    }

    void Start()
    {
        /**
         * Initialize goals that can be instantiated by Colonists directly.
         **/
        personalGoalPool = new List<Type>();
        personalGoalPool.AddRange(ColonyManager.inst.goalPool);
        personalGoalPool.AddRange(state.role.roleGoals);
        personalGoalPool = personalGoalPool.OrderBy((g) => {
            Goal goal = (Goal)Activator.CreateInstance(g);
            return (int)goal.type;
        }).ToList();
    }

    // Update is called once per frame
    void Update()
    {
        OnGameTick();
    }

    void UpdateState()
    {
        state.position = transform.position;
    }

    /**
     * This function should contain everything that we want to be rechecked every
     * in-game Tick. This is useful incase we want to have things update less frequently than
     * once every frame.
     **/
    void OnGameTick()
    {
 
        if(NeedsGoal)
        {
            ChooseGoal();
            UpdateCurrentGoal();
        }

        //Update Colonists' state based on the currentTask, as well as progress the Task
        if (CurrentAction != null)
        {
            if (CurrentAction.state != BaseAction.ActionState.Started && CurrentAction.state != BaseAction.ActionState.Completed)
            {
                CurrentAction.OnStart();
            }
                
            CurrentAction.PreTick();
            CurrentAction.OnTick();
        }

        state.position = transform.position;
    }

    public void ChooseGoal()
    {
        foreach (Type goalType in ColonyManager.inst.goalPool)
        {
            Goal newGoal = (Goal)Activator.CreateInstance(goalType);
            if (newGoal.Evaluate(this))
            {
                
                newGoal.doer = this;
                goalQueue.Enqueue(newGoal, 1000 - (int)newGoal.type);
                return;
            }
        }

        //If no goal applies, just wander.
        Vector2 ranCirc = UnityEngine.Random.insideUnitCircle * 10;
        Vector3 wanderDest = new Vector3(ranCirc.x, transform.position.y, ranCirc.y);

        goalQueue.Enqueue(new DProx(this, false, wanderDest), 0);
    }

    public void UpdateCurrentGoal()
    {
        if (!NeedsGoal && CurrentGoal.value.state != Goal.GoalState.Started)
        {
            Debug.LogFormat("Executing Goal {0}", CurrentGoal.value.GetType());
            Planner.BuildPlan(this, CurrentGoal.value);
            //CurrentGoal.value.Execute(false);

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
    public async void Distract(Goal perscription)
    {
        if (!NeedsGoal)
            CurrentGoal.value.InterruptAction();
        goalQueue.Enqueue(perscription, goalQueue.First.priority + 1);
        UpdateCurrentGoal();
    }

    public void SetStaticAvoidance()
    {
        mover.avoidancePriority = staticAvoidance;
    }

    public void SetMobileAvoidance()
    {
        mover.avoidancePriority = mobileAvoidance;
    }
}