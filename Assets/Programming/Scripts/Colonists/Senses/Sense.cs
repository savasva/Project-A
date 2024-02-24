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
        rangeTransforms = new List<Transform>();
        rangeInteractables = new List<IInteractable>();
        owner = GetComponent<Colonist>();

        //Make sure our IInteractable detector doesn't collide
        GetComponent<SphereCollider>().isTrigger = true;
    }

    protected abstract List<IInteractable> Scan(); 

    protected abstract bool IsValid(Transform interactable);
}
