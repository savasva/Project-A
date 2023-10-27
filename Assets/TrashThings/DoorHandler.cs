using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandler : MonoBehaviour
{
    public DoorObject[] Doors;

    public void ChangeDoorState(int id)
    {
        if (Doors[id].obstacle == true)
        {
            Doors[id].Open();
            return;
        }

        if (Doors[id].obstacle == false)
        {
            Doors[id].Close();
            return;
        }
    }
}
