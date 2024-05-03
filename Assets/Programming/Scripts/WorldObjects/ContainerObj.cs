using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Serialization;
using System;

public class ContainerObj : WorldObject
{
    public Inventory contents = new();
    public int capacity;

    public bool IsEmpty { get { return contents.Count <= 0; } }

    public bool HasEnough(InventoryItem item, int amount) {
        return contents[item.name].count > amount;
    }

    public bool Consume(InventoryItem item, int amount)
    {
        if (contents[item.name] == null || contents[item.name].count < amount) return false;

        contents[item.name].count -= amount;

        return true;
    }

    public class VendAction : BaseAction
    {
        public ContainerObj vendor;
        public Consumable target;

        public override Condition[] preconditions
        {
            get => new Condition[] {
                new Condition((ColonistState colState, WorldObjInfo objInfo) =>
                {
                    if (!vendor.contents.ContainsKey(target.name) || vendor.contents[target.name].count == 0) return float.MinValue;

                    return -ActionHelpers.Proximity(colState, vendor);
                })
            };
        }

        public VendAction() : base() { }

        public VendAction(Colonist _doer, string _name, ContainerObj _vendor, Consumable _target) : base(_doer, _name)
        {
            vendor = _vendor;
            target = _target;
        }

        public override void OnStart()
        {
            base.OnStart();
            
            Complete();
        }

        public override (float, BaseAction, ColonistState) PredictFit(Func<ColonistState, WorldObjInfo, float> predicate, ColonistState examinee)
        {
            (float, BaseAction, ColonistState) result = (float.MinValue, null, ColonistState.none);

            foreach (KeyValuePair<string, InventorySlot> slot in vendor.contents)
            {
                //Make changes
                examinee.inventory.Add(slot.Value.item);

                //Test fit
                float fit = predicate(examinee, WorldObjInfo.none);
                if (fit > result.Item1)
                {
                    result = (fit, new VendAction(null, string.Format("Get {0} from {1}", slot.Value.item, vendor.name), vendor, (Consumable)slot.Value.item), examinee);
                }

                //Revert changes
                examinee.inventory.Remove(slot.Value.item);
            }

            Debug.Log(vendor);
            Debug.Log(vendor.contents.Count);

            return result;
        }
    }
}
