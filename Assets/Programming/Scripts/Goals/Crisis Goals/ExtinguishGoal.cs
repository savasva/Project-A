using System;

[Serializable]
public class ExtinguishGoal : Goal
{
    WorldObject obj;

    /// <summary>
    /// Should be true if object is on fire
    /// </summary>
    public override Func<ColonistState, float> activationFit
    {
        get => (ColonistState state) => obj.state.aflame ? 1f : -1f;
    }

    /// <summary>
    /// Should be true if the object is NOT on fire
    /// </summary>
    public override Func<ColonistState, float> resultFit
    {
        get => (ColonistState state) => obj.state.aflame ? -1f : 1f;
    }

    public override bool Evaluate(ColonistState state)
    {
        return state.needs.tiredness >= 0.75f;
    }

    public ExtinguishGoal() : base()
    {
        type = GoalTypes.Crisis;
    }

    public ExtinguishGoal(Colonist _colonist, WorldObject _obj) : base(string.Format("Extinguish {0}.", _obj.name), _colonist, GoalTypes.Crisis) { }
}
