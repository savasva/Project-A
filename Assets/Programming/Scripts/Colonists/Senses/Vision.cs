using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Vision : Sense
{
    [SerializeField]
    LayerMask targetLayers;
    [SerializeField]
    LayerMask obstacleLayer;
    [SerializeField]
    private float range;
    [SerializeField]
    [Range(0f, 360f)]
    private float fov;

    private void Awake()
    {
        StartCoroutine(ScanLoop(0.5f));
    }

    private IEnumerator ScanLoop(float delay)
    {
        while (true)
        {
            rangeInteractables = Scan();
            yield return new WaitForSeconds(delay);
        }
    }

    protected override List<IInteractable> Scan()
    {
        List<IInteractable> valids = new List<IInteractable>();

        Collider[] inRange = Physics.OverlapSphere(transform.position, range, targetLayers.value);
        rangeTransforms = inRange.Select((Collider c) => { return c.transform; }).ToList();

        foreach (Transform t in rangeTransforms)
        {
            if (IsValid(t))
            {
                valids.Add(t.GetComponent<IInteractable>());
            }
        }

        return valids;
    }

    protected override bool IsValid(Transform interactable)
    {
        return (Vector3.Angle(transform.forward, interactable.transform.position - transform.position) < fov / 2) && !Physics.Raycast(transform.position, interactable.position - transform.position, Vector3.Distance(interactable.position, transform.position), obstacleLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + DirFromAngle(-fov / 2, false) * range);
        Gizmos.DrawLine(transform.position, transform.position + DirFromAngle(fov / 2, false) * range);

        foreach (Transform t in rangeTransforms)
        {
            if (IsValid(t))
                Gizmos.color = Color.green;
            else
                Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(t.transform.position, 1);
            Gizmos.DrawRay(transform.position, t.position - transform.position);
        }
    }

    private Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
