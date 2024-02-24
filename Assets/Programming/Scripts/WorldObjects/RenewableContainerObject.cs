using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenewableContainerObject : ContainerObject
{
    public float renewTime;
    public InventoryItem renewResults;

    public class RenewAction : BaseAction
    {
        [SerializeField]
        RenewableContainerObject container;
        float currRenewTime = 0;

        public override Func<ColonistState, float> precondition
        {
            get => (ColonistState state) => {
                return -container.Proximity(state);
            };
        }

        public RenewAction(RenewableContainerObject _machine)
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

        public override (float, BaseAction, ColonistState) PredictFit(Func<ColonistState, float> predicate, ColonistState examinee)
        {
            return (predicate(examinee), new RenewAction(container), examinee);
        }
    }
}
