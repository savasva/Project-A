using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyManager : MonoBehaviour
{
    public static EmergencyManager inst;

    [SerializeField]
    bool activeEmergency = false;

    private void Awake()
    {
        if (inst == null)
            inst = this;
        else if (inst != this)
            Destroy(this);
    }


    
}
