using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public abstract class Sense : MonoBehaviour
{
    protected Colonist owner;
    protected List<IInteractable> rangeInteractables;
    [SerializeField]
    protected List<Transform> rangeTransforms;
    [SerializeField]
    protected List<WorldObject> rangeObjs; 

    private void Start()
    {
        rangeTransforms = new();
        rangeInteractables = new();
        owner = GetComponent<Colonist>();
    }

    [Button("Scan")]
    public abstract List<IInteractable> Scan(); 

    protected abstract bool IsValid(Transform interactable);
}
