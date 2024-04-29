using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraObject : WorldObject
{
    [Header("Camera Properties")]

    [SerializeField]
    Camera cam;
    public bool accessible;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    public void Toggle()
    {
        cam.enabled = !cam.enabled;
    }

    public override void OnUse()
    {
        CameraManager.inst.UpdateCurrentCam(this);
    }
}