using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class WorldObjectProperty
{
    public abstract List<BaseAction> propActions { get; }
}
