using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Condition
{
    public IComparable threshold;
    public Comparison comparer;

    public Condition(IComparable _threshold, Comparison _comparer)
    {
        threshold = _threshold;
        comparer = _comparer;
    }

    /*
     * True if the EndCondition is met, False otherwise.
     */
    public bool Compare(ColonistState colState)
    {
        IComparable other = null;
        foreach (FieldInfo fieldInfo in colState.GetType().GetFields())
        {
            if (fieldInfo.FieldType == threshold.GetType())
            {
                other = (IComparable)typeof(Colonist).GetField(fieldInfo.Name).GetValue(colState);
            }
        }

        if (other == null) return false;

        return threshold.Evaluate(other, comparer);
    }

    public static implicit operator Condition[](Condition c) => new Condition[] { c };

    public enum Comparison
    {
        Above,
        Below
    }
}
