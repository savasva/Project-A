using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColonistIndicator : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;
    private void FixedUpdate()
    {
        transform.position = target.position + offset;
    }
}
