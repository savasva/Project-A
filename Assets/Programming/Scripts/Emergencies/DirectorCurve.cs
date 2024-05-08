using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Director Curve", menuName = "Project A/Director Curve")]
public class DirectorCurve : ScriptableObject
{
    public AnimationCurve curve;
}
