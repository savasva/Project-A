using Sirenix.OdinInspector.Editor.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Colonist : MonoBehaviour
{
    [SerializeField]
    Plan currentPlan;

    [Header("\"Who I am\" Variables")]
    public ColonistModel model;
    public ColonistState state;
    public Big5Personality personality;

    [Header("Memory")]
    //Short-Term Memory / Working Memory
    public MemoryContainer stm;
    //Long-Term Memory
    public MemoryContainer ltm;

    BaseAction CurrentAction {
        get {
            //Debug.Log(CurrentGoal.value.NeedsSubgoal);
            if (NeedsPlan) return null;

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
            if (NeedsPlan) return null;

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

    public bool NeedsGoal
    {
        get {
            return (CurrentGoal == null || CurrentGoal.value == null);
        }
    }

    public bool NeedsPlan {
        get {
            return NeedsGoal || CurrentGoal.value.plan.stack.Count == 0;
        }
    }
    public List<Goal> goalQueueVisualizer;

    List<IInteractable> knownInteractables;

    [Header("Pathfinding")]
    public NavMeshAgent mover;
    int mobileAvoidance;
    [SerializeField]
    int staticAvoidance;

    List<Goal> personalGoalPool;
    Sense[] senses;

    private void Awake()
    {
        mover = GetComponent<NavMeshAgent>();

        //actionQueue = new DoubleEndedQueue<BaseAction>();
        goalQueue = new PriorityQueue<Goal>();
        senses = GetComponents<Sense>();
        mobileAvoidance = mover.avoidancePriority;
        UpdateState();
    }

    private void Start()
    {
        /**
         * Initialize goals that can be instantiated by Colonists directly.
         **/
        personalGoalPool = new();
        personalGoalPool.AddRange(ColonyManager.inst.GlobalGoalPool);
        personalGoalPool.AddRange(state.role.Goals);
    }

    // Update is called once per frame
    private void Update()
    {
        OnGameTick();
    }

    private void LateUpdate()
    {
        AfterGameTick();
    }

    private void UpdateState()
    {
        state.position = transform.position;
    }

    private List<Goal> GoalPool {
        get
        {
            List<Goal> pool = new List<Goal>(personalGoalPool);
            
            foreach (Sense sense in senses)
            {
                Debug.Log(sense.GetType().Name);
                foreach (IInteractable obj in sense.Scan())
                {
                    pool.AddRange(obj.Goals);
                }
            }

            pool = pool.OrderBy((g) => {
                return (int)g.GoalType;
            }).ToList();

            return pool;
        }
    }

    /**
     * This function should contain everything that we want to be rechecked every
     * in-game Tick. This is useful incase we want to have things update less frequently than
     * once every frame.
     **/
    private void OnGameTick()
    {
        //Update Colonists' state based on the currentTask, as well as progress the Task
        if (CurrentAction != null)
        {
            UpdateCurrentAction();
        }

        state.position = transform.position;
    }

    private void AfterGameTick()
    {
        if (NeedsGoal)
        {
            Debug.Log("Needs goal!");
            ChooseGoal();
            UpdateCurrentGoal();
        }
    }

    public void ChooseGoal()
    {
        foreach (Goal goal in GoalPool)
        {
            if (goal.Evaluate(state)) {
                //TODO: Implement COPY function
                Goal newGoal = goal.DeepCopy();
                if (goal.GetType() == typeof(ExtinguishGoal))
                {
                    ExtinguishGoal eGoal = (ExtinguishGoal)goal;
                    Debug.Log(eGoal.obj);
                }
                newGoal.doer = this;
                goalQueue.Enqueue(newGoal, 1000 - (int)newGoal.GoalType);
                return;
            }
        }

        //If no goal applies, just wander.
        /*Vector2 ranCirc = UnityEngine.Random.insideUnitCircle * 10;
        Vector3 wanderDest = new Vector3(ranCirc.x, transform.position.y, ranCirc.y);

        goalQueue.Enqueue(new DProx(this, false, wanderDest), 0);*/
    }

    /// <summary>
    /// Activates highest priority goal.
    /// </summary>
    private void UpdateCurrentGoal()
    {
        if (!NeedsGoal && CurrentGoal.value.state != Goal.GoalState.Started)
        {
            Debug.LogFormat("Executing Goal {0}", CurrentGoal.value.GetType());
            CurrentGoal.value.SetPlan(Planner.BuildPlan(this, CurrentGoal.value));
            //CurrentGoal.value.Execute(false);
        }

        goalQueueVisualizer = goalQueue.ToList();
    }

    public void UpdateCurrentAction()
    {
        if (CurrentAction.state != BaseAction.ActionState.Started && CurrentAction.state != BaseAction.ActionState.Completed)
        {
            CurrentAction.OnStart();
        }

        CurrentAction.PreTick();
        CurrentAction.OnTick();
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

    public void AddInteractable(IInteractable interactable)
    {
        knownInteractables.Add(interactable);
    }

    public void RemoveInteractable(IInteractable interactable)
    {
        knownInteractables.Remove(interactable);
    }
}