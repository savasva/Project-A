using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Project A/Consumable")]
public class Consumable : ScriptableObject
{
    public float consumeTime;
    public Needs nourishment;

    public Consumable(float _consTime, Needs _nourishment)
    {
        consumeTime = _consTime;
        nourishment = _nourishment;
    }
}
