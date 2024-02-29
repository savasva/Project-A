using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlamableProperty : WorldObjectProperty
{
    [Range(0f, 1f)]
    public float burnProgress;

    public override List<BaseAction> propActions => new List<BaseAction>() {
        
    };
}
