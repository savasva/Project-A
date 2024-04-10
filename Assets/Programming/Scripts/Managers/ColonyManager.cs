﻿using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;

public class ColonyManager : MonoBehaviour
{
    public static ColonyManager inst;

    /*
     * Dictionary to cache our colonists. Since they will be accessed often in
     * OnTriggerEnter and OnTriggerEnter on WorldObjects, this will help performance.
     */
    public Dictionary<Role, Colonist> colonists = new Dictionary<Role, Colonist>();

    public Goal[] GlobalGoalPool {
        get {
            return new Goal[]
            {
                new SleepGoal(),
                new EatGoal()
            };
        }
    }

    [Header("Gameplay")]
    public float currentEnergy;
    public float maxEnergy = 100;

    [Header("Background Info")]
    public List<WorldItem> worldItems;
    public WorldObjectCollection worldObjects;
    public WorldObjectCollection eatObjects;
    public WorldObjectCollection sleepObjects;
    public WorldObjectCollection workObjects;
    public WorldObjectCollection cainTerminals;
    public WorldObjectCollection flamableObjects;
    public Consumable[] consumables;

    private void Awake()
    {
        if (inst == null)
            inst = this;
        else if (inst != this)
            Destroy(this);

        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        PopulateWorld();
        currentEnergy = maxEnergy;
    }

    public void PopulateWorld()
    {
        foreach (WorldObject obj in worldObjects.objects)
        {
            obj.info.InitProperties(obj);
        }

        foreach(Colonist col in FindObjectsByType<Colonist>(FindObjectsSortMode.None))
        {
            colonists.Add(col.state.role, col);
        }

        PopulateKnowledgeBackground();

        Debug.Log("Objects initialized and cached!");
    }

    //O(n)
    public Colonist GetBestColonist(List<Colonist> colonistPool, Big5Personality bias)
    {
        Colonist bestColonist = null;
        float bestWeight = float.MinValue;

        foreach (Colonist entry in colonistPool)
        {
            float weight = (entry.personality * bias).Magnitude();
            if (weight > bestWeight)
            {
                bestWeight = weight;
                bestColonist = entry;
            }
        }

        return bestColonist;
    }

    //O(n)
    public Colonist GetBestColonist(Big5Personality bias)
    {
        Colonist bestColonist = null;
        float bestWeight = float.MinValue;

        foreach(KeyValuePair<Role, Colonist> entry in colonists)
        {
            float weight = (entry.Value.personality * bias).Magnitude();
            if (weight > bestWeight)
            {

                bestColonist = entry.Value;
            }
        }

        return bestColonist;
    }

    public Colonist GetColonist(Predicate<Colonist> filter = null)
    {
        List<KeyValuePair<Role, Colonist>> cols = colonists.ToList();

        if (filter != null)
        {
            cols = cols.Where(c => filter(c.Value)).ToList();
        }

        int index = UnityEngine.Random.Range(0, cols.Count);
        Colonist chosenOne = cols[index].Value;

        return chosenOne;
    }

    public Colonist GetColonistByRole(Role role)
    {
        return colonists[role];
    }

    //O(n) with a lot of overhead from FindObjectsOfType
    public void PopulateKnowledgeBackground()
    {
        worldItems = FindObjectsByType<WorldItem>(FindObjectsSortMode.None).ToList();

        //TODO: May be better to populate this by hand, but it only rudns once so shrug emoji.
        worldObjects = new WorldObjectCollection(FindObjectsByType<WorldObject>(FindObjectsSortMode.None).ToList());

        /*
         * Setup scene information so workstations and such can be assigned.
         */
        sleepObjects = new WorldObjectCollection(worldObjects.objects.Where(obj => obj.benefit.tiredness < 0).ToList());
        eatObjects = new WorldObjectCollection(worldObjects.objects.Where(obj => obj.benefit.hunger < 0).ToList());
        cainTerminals = new WorldObjectCollection(worldObjects.objects.Where(obj => obj.GetType() == typeof(TerminalObject)).ToList());
        //workObjects = new WorldObjectCollection(worldObjects.objects.Where(obj => typeof(obj.taskType) == typeof(WorkTask)).ToList());
        flamableObjects = new WorldObjectCollection(worldObjects.objects.Where(obj => obj.info.GetProperty(typeof(FlamableProperty)) != null).ToList());
    }

    //TODO: Using WorldObject as a parameter creates a cylic dependency between Colonist and WorldObject. This my bite us in the ass later.
    public static PTRANS BuildLinePTRANS(Colonist col, WorldObject target)
    {
        Vector3 dir = target.transform.forward;
        int linePos = target.queue.IndexOf(col);

        if (linePos == -1)
        {
            linePos = target.queue.Count;
        }

        Vector3 dest = target.GetDestination() + (dir * linePos * 2.5f);
        dest.y = col.transform.position.y;

        return new PTRANS(col, string.Format("Moving to {0}", target.GetGameObject().name), dest);
    }
}
