using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SocializeGoal : Goal
{
    public string query;
    public Colonist conversant;
    public List<Colonist> failedCols;


    public override void OnStart()
    {
        base.OnStart();
    }

    public override void OnTick()
    {
        if (conversant == null)
        {
            conversant = ColonyManager.inst.GetColonist(new Predicate<Colonist>(delegate (Colonist col)
            {
                return col != owner;
            }));
        }
    }
}
