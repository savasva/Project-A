using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticCameraController : MonoBehaviour
{

    public List<CameraPointController> CameraPoints;

    public void SwitchCamera(int id)
    {
        Camera.main.transform.position = CameraPoints[id].transform.position + (CameraPoints[id].transform.forward * 0.5f);
        Camera.main.transform.rotation = CameraPoints[id].transform.rotation;
    }

}
