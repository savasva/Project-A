using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ConditionSet
{
    public Condition[] set;
    public Inclusivity inclusivity;

    public ConditionSet() { }

    public ConditionSet(Condition condition)
    {
        inclusivity = Inclusivity.OR;
        set = condition;
    }

    public ConditionSet(Inclusivity _inclusivity, params Condition[] conditions)
    {
        inclusivity = _inclusivity;
        set = conditions;
    }

    /*public bool IsFulfilled(IComparable comparison)
     {
         switch(inclusivity)
         {
             case Inclusivity.AND:
                 foreach (Condition condition in set)
                 {
                     if (!condition.Compare(comparison))
                         return false;
                 }
                 return true;

             case Inclusivity.OR:
                 foreach (Condition condition in set)
                 {
                     if (condition.Compare(comparison))
                         return true;
                 }
                 return false;

             case Inclusivity.XOR:
                 int trueCount = 0;
                 foreach (Condition condition in set)
                 {
                     if (condition.Compare(comparison))
                         trueCount++;

                     if (trueCount > 1)
                         return false;
                 }

                 return trueCount == 1;

             default:
                 return false;
         }

     }*/

    public bool IsFulfilled(ColonistState target)
    {
        if (set.Length == 0) return true;

        switch (inclusivity)
        {
            case Inclusivity.AND:
                foreach (Condition condition in set)
                {
                    if (!condition.Compare(target))
                        return false;
                }
                return true;

            case Inclusivity.OR:
                foreach (Condition condition in set)
                {
                    if (condition.Compare(target))
                        return true;
                }
                return false;

            case Inclusivity.XOR:
                int trueCount = 0;
                foreach (Condition condition in set)
                {
                    if (condition.Compare(target))
                        trueCount++;

                    if (trueCount > 1)
                        return false;
                }

                return trueCount == 1;

            default:
                return false;
        }

    }

    public static implicit operator ConditionSet[](ConditionSet c) => new ConditionSet[] { c };

    public enum Inclusivity
    {
        AND,
        OR,
        XOR
    }
}
