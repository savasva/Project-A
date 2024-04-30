using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : ScriptableObject
{
    /// <summary>
    /// Compile all necessary information in order to produce
    /// </summary>
    public abstract string EncodeMessage();
}
