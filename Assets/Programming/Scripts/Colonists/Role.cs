using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public abstract class Role : IInteractable
{
    public Color color;
    public virtual Type[] roleGoals {
        get => new Type[0];
    }

    public Vector3 GetDestination()
    {
        return GetGameObject().transform.position;
    }

    public GameObject GetGameObject()
    {
        Colonist matchingColonist = ColonyManager.inst.GetColonistByRole(this);
        return matchingColonist.gameObject;
    }
}

public class EngineerRole : Role
{
    public override Type[] roleGoals
    {
        get => new Type[0];
    }
}

public class XenobioRole : Role
{
    public override Type[] roleGoals
    {
        get => new Type[0];
    }
}