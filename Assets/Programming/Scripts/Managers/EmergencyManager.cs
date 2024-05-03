using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EmergencyManager : MonoBehaviour
{
    public static EmergencyManager inst;

    [SerializeField]
    bool activeEmergency = false;

    [SerializeField]
    [Range(0, 1f)]
    float fireIntensity;

    private void Awake()
    {
        if (inst == null)
            inst = this;
        else if (inst != this)
            Destroy(this);
    }

    private void Start()
    {
        
    }

    [Button]
    void StartFire()
    {
        FireEmergency test = new FireEmergency(fireIntensity);
    }
}
