using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WorldObjectProperty
{
    public WorldObject obj;
    public virtual BaseAction[] PropActions { get; }
    public virtual Goal[] PropGoals { get; }

    public void InitProperty(WorldObject owner)
    {
        obj = owner;
    }
}
