using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Needs
{
    [Range(-1, 1)]
    public float hunger;

    [Range(-1, 1)]
    public float thirst;

    /**
     * -1 would be Well Rested, 0 would be Rested, and 1 would be Spent
     * Maybe pass out when tiredness is 1?
     **/
    [Range(-1, 1)]
    public float tiredness;

    [Range(-1, 1)]
    public float boredom;

    [Range(-1, 1)]
    public float stress;

    public Needs self { get { return this; } }

    public Needs(float _hunger, float _thirst, float _tiredness, float _boredom, float _stress)
    {
        hunger = _hunger;
        thirst = _thirst;
        tiredness = _tiredness;
        boredom = _boredom;
        stress = _stress;
    }

    public float Value()
    {
        return hunger + thirst + tiredness + boredom + stress;
    }

    public static float Difference(Needs before, Needs after)
    {
        return (after - before).Value();
    }

    public void Clamp()
    {
        hunger = Mathf.Clamp(hunger, -1, 1);
        thirst = Mathf.Clamp(thirst, -1, 1);
        tiredness = Mathf.Clamp(tiredness, -1, 1);
        boredom = Mathf.Clamp(boredom, -1, 1);
        stress = Mathf.Clamp(stress, -1, 1);
    }

    public static Needs operator +(Needs a, Needs b) {
        Needs aggregate = new Needs();

        aggregate.hunger = a.hunger + b.hunger;
        aggregate.thirst = a.thirst + b.thirst;
        aggregate.tiredness = a.tiredness + b.tiredness;
        aggregate.boredom = a.boredom + b.boredom;
        aggregate.stress = a.stress + b.stress;

        return aggregate;
    }

    public static Needs operator -(Needs a, Needs b)
    {
        Needs aggregate = new Needs();

        aggregate.hunger = a.hunger - b.hunger;
        aggregate.thirst = a.thirst - b.thirst;
        aggregate.tiredness = a.tiredness - b.tiredness;
        aggregate.boredom = a.boredom - b.boredom;
        aggregate.stress = a.stress - b.stress;

        return aggregate;
    }

    public static Needs operator *(Needs a, float b)
    {
        Needs aggregate = new Needs();

        aggregate.hunger = a.hunger * b;
        aggregate.thirst = a.thirst * b;
        aggregate.tiredness = a.tiredness * b;
        aggregate.boredom = a.boredom * b;
        aggregate.stress = a.stress * b;

        return aggregate;
    }

    public static bool operator <(Needs a, Needs b)
    {
        return (a.hunger < b.hunger) && (a.thirst < b.thirst) && (a.tiredness < b.tiredness) && (a.boredom < b.boredom) && (a.stress < b.stress);
    }

    public static bool operator >(Needs a, Needs b)
    {
        return (a.hunger > b.hunger) && (a.thirst > b.thirst) && (a.tiredness > b.tiredness) && (a.boredom > b.boredom) && (a.stress > b.stress);
    }

    public override string ToString()
    {
        return string.Format("Hunger: {0} | Thirst: {1} | Tiredness: {2} | Boredom: {3} | Stress: {4}", hunger, thirst, tiredness, boredom, stress);
    }
}
