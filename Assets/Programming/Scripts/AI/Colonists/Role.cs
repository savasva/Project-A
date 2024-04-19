using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Role : IInteractable
{
    public Color color;
    public virtual List<Goal> Goals {
        get => new List<Goal>();
    }

    public BaseAction[] Actions => new BaseAction[0];

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

}

public class XenobioRole : Role
{

}