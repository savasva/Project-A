using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IComparable
{
    public abstract bool Evaluate(IComparable other, Condition.Comparison comparison);
}
