using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Needs : IComparable
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

    public Needs self { get { return this; } }

    public Needs(float _hunger, float _thirst, float _tiredness)
    {
        hunger = _hunger;
        thirst = _thirst;
        tiredness = _tiredness;
    }

    public void Clamp()
    {
        hunger = Mathf.Clamp(hunger, -1, 1);
        thirst = Mathf.Clamp(thirst, -1, 1);
        tiredness = Mathf.Clamp(tiredness, -1, 1);
    }

    public static Needs operator +(Needs a, Needs b) {
        Needs aggregate = new Needs();

        aggregate.hunger = a.hunger + b.hunger;
        aggregate.thirst = a.thirst + b.thirst;
        aggregate.tiredness = a.tiredness + b.tiredness;

        return aggregate;
    }

    public static Needs operator -(Needs a, Needs b)
    {
        Needs aggregate = new Needs();

        aggregate.hunger = a.hunger - b.hunger;
        aggregate.thirst = a.thirst - b.thirst;
        aggregate.tiredness = a.tiredness - b.tiredness;

        return aggregate;
    }

    public static Needs operator *(Needs a, float b)
    {
        Needs aggregate = new Needs();

        aggregate.hunger = a.hunger * b;
        aggregate.thirst = a.thirst * b;
        aggregate.tiredness = a.tiredness * b;

        return aggregate;
    }

    public static bool operator <(Needs a, Needs b)
    {
        return (a.hunger < b.hunger) && (a.thirst < b.thirst) && (a.tiredness < b.tiredness);
    }

    public static bool operator >(Needs a, Needs b)
    {
        return (a.hunger > b.hunger) && (a.thirst > b.thirst) && (a.tiredness > b.tiredness);
    }

    public override string ToString()
    {
        return string.Format("Hunger: {0}\nThirst: {1}\nTiredness: {2}", hunger, thirst, tiredness);
    }

    public bool Evaluate(IComparable other, Condition.Comparison comparison)
    {
        if (other.GetType() != typeof(Needs))
        {
            Debug.LogError("Evaluations \"other\" needs to be of same type as operand");
            return false;
        }

        Needs operand = (Needs)other;


        switch(comparison)
        {
            case Condition.Comparison.Above:
                return self > operand;

            case Condition.Comparison.Below:
                return self < operand;
        }

        return false;
    }
}
