using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable
{
    public float consumeTime;
    public Needs nourishment;

    public Consumable(float _consumeTime, Needs _nourishment)
    {
        consumeTime = _consumeTime;
        nourishment = _nourishment;
    }
}
