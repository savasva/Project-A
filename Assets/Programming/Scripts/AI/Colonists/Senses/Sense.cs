using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Sense : MonoBehaviour
{
    protected Colonist owner;
    protected List<IInteractable> rangeInteractables;
    [SerializeField]
    protected List<Transform> rangeTransforms;

    private void Start()
    {
        rangeTransforms = new();
        rangeInteractables = new();
        owner = GetComponent<Colonist>();
    }

    public abstract List<IInteractable> Scan(); 

    protected abstract bool IsValid(Transform interactable);
}
