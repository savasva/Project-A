using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

[System.Serializable]
public class WorldObjectCollection
{
    public List<WorldObject> objects;

    public WorldObjectCollection()
    {
        objects = new List<WorldObject>();
    }

    public WorldObjectCollection(List<WorldObject> _objects)
    {
        objects = _objects;
    }

    public WorldObject GetFreestObject()
    {
        int shortestLine = 100000;
        int bestIndex = 0;

        for(int i = 0; i < objects.Count; i++)
        {
            if (objects[i].queue.Count < shortestLine)
            {
                shortestLine = objects[i].queue.Count;
                bestIndex = i;
            }
        }

        return objects[bestIndex];
    }
    public WorldObject GetFreeObject()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            if (objects[i].queue.Count == 0)
            {
                return objects[i];
            }
        }

        return null;
    }

    public void Push(WorldObject obj)
    {
        objects.Add(obj);
    }

    //O(n^2) I think? Kinda sucks.
    //TODO: Make IEnumerator that calculates X paths per frame. (yield return new WaitForEndOfFrame())
    public WorldObject GetNearestObject(Colonist col)
    {
        int closestIndex = 0;
        float closestDist = 10000;

        if (objects.Count == 0) return null;

        if (objects.Count == 1) return objects[0];

        for (int i = 0; i < objects.Count; i++)
        {
            WorldObject obj = objects[i];
            Vector3 targetPos = (obj.moveDestination != null) ? obj.moveDestination.position : obj.transform.position;

            NavMeshPath path = new NavMeshPath();
            col.mover.CalculatePath(targetPos, path);

            float dist = CalculatePathLength(path);

            if (dist < closestDist)
            {
                closestDist = dist;
                closestIndex = i;
            }
        }

        return objects[closestIndex];
    }

    //O(n)
    /*public WorldObject GetTargetedObject(TaskTarget targetBias)
    {
        Colonist bestColonist = null;
        float bestWeight = float.MinValue;

        foreach (WorldObject obj in objects)
        {
            float weight = (obj.personality * targetBias).Magnitude();
            if (weight > bestWeight)
            {
                bestWeight = weight;
                bestColonist = entry;
            }
        }

        return bestColonist;
    }*/

    float CalculatePathLength(NavMeshPath path)
    {

        // Create a float to store the path length that is by default 0.
        float pathLength = 0;

        // Increment the path length by an amount equal to the distance between each waypoint and the next.
        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            pathLength += Vector3.Distance(path.corners[i], path.corners[i + 1]);
        }

        return pathLength;
    }
}
