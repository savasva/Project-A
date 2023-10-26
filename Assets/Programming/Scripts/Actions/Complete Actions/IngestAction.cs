using System;
using UnityEngine;

//INGEST
[System.Serializable]
public class IngestAction : BaseAction
{
    public Consumable consumable;
    float elapsedTime;

    public IngestAction(string _name, Consumable _toConsume) : base(_name)
    {
        consumable = _toConsume;
    }

    public override void OnTick()
    {
        if (elapsedTime >= consumable.consumeTime)
        {
            doer.needs += consumable.nourishment;
            CompleteTask();
        }
        elapsedTime += Time.deltaTime;
    }
}
