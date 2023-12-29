using UnityEngine;
using System.Collections;

[System.Serializable]
public struct ColonistState
{
    public Vector3 position;
    public Needs needs;
    [SerializeReference]
    public Role role;

    public override string ToString()
    {
        return string.Format("\n{0}\n{1}\n{2}", position, needs, role.GetType());
    }
}
