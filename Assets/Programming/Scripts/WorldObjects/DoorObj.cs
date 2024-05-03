using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorObj : WorldObject
{
    public void Open()
    {
        obstacle = false;
        GetComponent<Animator>().SetTrigger("Open");
    }

    public void Close()
    {
        obstacle = true;
        GetComponent<Animator>().SetTrigger("Close");
    }

    /*void OnTriggerEnter(Collider other)
    {
        if (obstacle && other.CompareTag("Colonist"))
        {
            Colonist col = other.GetComponent<Colonist>();
            if (col.currentAction.GetType() == typeof(PTRANS))
            {
                ((PTRANS)col.currentAction).HandleObstacle(this);
                Debug.Log("Handle obstacle!");
            }
        }
    }*/
}
