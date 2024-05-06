using System.Collections;
using System.Collections.Generic;
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

    [Header("Planning")]
    [SerializeField]
    float goalDelay = 0.15f;

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

    [SerializeField]
    GoalPQueue goalQueue;
    public GoalPQueue.PriorityNode CurrentGoal {
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

    List<IInteractable> knownInteractables;

    [Header("Pathfinding")]
    public NavMeshAgent mover;
    int mobileAvoidance;
    [SerializeField]
    int staticAvoidance;

    List<Goal> personalGoalPool;
    public Sense[] senses;

    private void Awake()
    {
        mover = GetComponent<NavMeshAgent>();

        //actionQueue = new DoubleEndedQueue<BaseAction>();
        goalQueue = new GoalPQueue();
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

        StartCoroutine(ScanForGoals(goalDelay));
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
                foreach (IInteractable obj in sense.Scan())
                {
                    pool.AddRange(obj.Goals);
                }
            }

            /*pool = pool.OrderBy((g) => {
                return (int)g.GoalType;
            }).ToList();*/

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
            //UpdateValidGoals();
            //Debug.Log("Needs goal!");
            //_currentGoal = GetValidGoals()[0];
            //UpdateCurrentGoal();
        }
    }

    public void UpdateValidGoals()
    {
        while (goalQueue.Count > 0)
        {
            goalQueue.Dequeue();
        }

        foreach (Goal goal in GoalPool)
        {
            bool eval = goal.Evaluate(state);
            if (eval) {
                goalQueue.Enqueue(goal.DeepCopy(), (int)goal.GoalType);
            }

            Debug.LogFormat("<b><color=red>{0}:</color></b> {1} is {2}", model.name, goal.GetType(), eval ? "VALID" : "INVALID");
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
            CurrentGoal.value.doer = this;
            CurrentGoal.value.SetPlan(Planner.BuildPlan(this, CurrentGoal.value));
            //CurrentGoal.value.Execute(false);
        }
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

    IEnumerator ScanForGoals(float scanFreq)
    {
        while (true)
        {
            UpdateValidGoals();
            UpdateCurrentGoal();
            yield return new WaitForSeconds(scanFreq);
        }
    }
}