using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColonistIndicator : MonoBehaviour
{
    [SerializeField] Colonist target;
    [SerializeField] Vector3 offset;

    public void Init(Colonist _target)
    {
        target = _target;
        GetComponent<Image>().sprite = target.model.icon;
    }

    private void FixedUpdate()
    {
        transform.position = target.transform.position + offset;
        if (CameraManager.inst != null && CameraManager.inst.CurrentCam != null)
            transform.LookAt(CameraManager.inst.CurrentCam.transform);
    }
}
