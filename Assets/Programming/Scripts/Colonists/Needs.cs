using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Needs
{
    [Range(-1, 1)]
    public float hunger = 0;

    [Range(-1, 1)]
    public float thirst = 0;

    /**
     * -1 would be Well Rested, 0 would be Rested, and 1 would be Spent
     * Maybe pass out when tiredness is 1?
     **/
    [Range(-1, 1)]
    public float tiredness = 0;

    public Needs self { get { return this; } }

    public Needs()
    {
        
    }

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
}
