using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : MonoBehaviour
{
    public WorldObjectCollection contents;

    private void Start()
    {
        contents.objects = GetComponentsInChildren<WorldObject>().ToList();
    }
}
