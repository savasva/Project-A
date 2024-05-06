using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : MonoBehaviour
{
    public WorldObjCollection contents;

    private void Start()
    {
        contents = new WorldObjCollection(GetComponentsInChildren<WorldObject>());
    }
}
