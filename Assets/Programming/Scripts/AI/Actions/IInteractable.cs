using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public interface IInteractable
{
    public Vector3 GetDestination();
    public GameObject GetGameObject();

    public BaseAction[] Actions { get; }

    public List<Goal> Goals { get; }

}
