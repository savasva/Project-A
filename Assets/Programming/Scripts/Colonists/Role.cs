using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "New Role", menuName = "Project A/Role", order = 1)]
public class Role : ScriptableObject, IInteractable
{
    [Header("Higher = More Important")]
    public int importance;
    //public Big5Personality bias;
    public Color color;
    public Needs benefitMultiplier;

    public Vector3 GetDestination()
    {
        
        return GetGameObject().transform.position;
    }

    public GameObject GetGameObject()
    {
        Colonist matchingColonist = ColonyManager.inst.GetColonistByRole(this);
        return matchingColonist.gameObject;
    }
}
