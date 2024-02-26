﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Serialization;
using System;

public class ContainerObject : WorldObject
{
    public Inventory contents;
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
        public ContainerObject vendor;
        public Consumable target;

        public override Func<ColonistState, float> precondition
        {
            get => (ColonistState state) =>
            {
                if (!vendor.contents.ContainsKey(target.name) || vendor.contents[target.name].count == 0) return float.MinValue;

                return -vendor.Proximity(state);
            };
        }

        public override Func<ColonistState, float> postcondition
        {
            get => (ColonistState state) => Needs.Difference(state.needs, state.needs + target.nourishment);
        }

        public VendAction() : base() { }

        public VendAction(Colonist _doer, string _name, ContainerObject _vendor, Consumable _target) : base(_doer, _name)
        {
            vendor = _vendor;
            target = _target;
        }

        public override void OnStart()
        {
            base.OnStart();
            
            Complete();
        }

        public override (float, BaseAction, ColonistState) PredictFit(Func<ColonistState, float> predicate, ColonistState examinee)
        {
            (float, BaseAction, ColonistState) result = (float.MinValue, null, ColonistState.none);

            foreach (KeyValuePair<string, InventorySlot> slot in vendor.contents)
            {
                examinee.inventory.Add(slot.Value.item);
                float fit = predicate(examinee);
                if (fit > result.Item1)
                {
                    result = (fit, new VendAction(null, string.Format("Get {0}", slot.Value.item), vendor, (Consumable)slot.Value.item), examinee);
                }
                examinee.inventory.Remove(slot.Value.item);
            }

            return result;
        }
    }
}
