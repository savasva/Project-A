using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager inst;

    CameraObject currentCam;
    //public CameraObject[] cameras;

    void Awake()
    {
        if (inst == null)
            inst = this;
        else if (inst != this)
            Destroy(this);

        DontDestroyOnLoad(this);
    }

    public void UpdateCurrentCam(CameraObject newCam)
    {
        if (currentCam != null)
        {
            currentCam.Toggle();
        }
        
        newCam.Toggle();
        currentCam = newCam;
    }
}
