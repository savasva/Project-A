using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenewableContainerObj : ContainerObj
{
    public float renewTime;
    public InventoryItem renewResults;

    public class RenewAction : BaseAction
    {
        [SerializeField]
        RenewableContainerObj container;
        float currRenewTime = 0;

        public override Condition[] preconditions
        {
            get => new Condition[] {
                new Condition((ColonistState colState, WorldObjInfo objInfo) => {
                    return -ActionHelpers.Proximity(colState, container);
                })
            };
        }

        public RenewAction(RenewableContainerObj _machine)
        {
            container = _machine;
        }

        public override void OnTick()
        {
            base.OnTick();

            if (currRenewTime > container.renewTime)
            {
                Complete();
            }

            currRenewTime += Time.deltaTime;
        }

        protected override void Complete()
        {
            for (int i = container.contents.Count; i < container.capacity; i++)
            {
                container.contents.Add(container.renewResults);
            }

            base.Complete();
        }

        public override (float, BaseAction, ColonistState) PredictFit(Func<ColonistState, WorldObjInfo, float> predicate, ColonistState examinee)
        {
            return (predicate(examinee, WorldObjInfo.none), new RenewAction(container), examinee);
        }
    }
}
