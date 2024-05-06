using UnityEngine;

[System.Serializable]
public class WorldObjComponent
{
    public string name;

    [Range(0, 1f)]
    public float durability;
}