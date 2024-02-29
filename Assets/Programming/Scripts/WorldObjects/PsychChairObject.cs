using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsychChairObject : WorldObject
{
    public class TherapyAction : BaseAction
    {
        [SerializeField]
        PsychChairObject seat;

        public override Func<ColonistState, WorldObjectInfo, float> precondition
        {
            get => (ColonistState colState, WorldObjectInfo objInfo) => {
                return -ActionHelpers.Proximity(colState, seat);
            };
        }

        public TherapyAction(PsychChairObject _seat): base() {
            seat = _seat;
        }

        public override void OnTick()
        {
            base.OnTick();

            doer.state.needs += seat.benefit * Time.deltaTime;

            if (doer.state.needs.stress <= -0.5f)
            {
                Complete();
            }
        }

        protected override void Complete()
        {
            doer.mover.ResetPath();
            base.Complete();
        }

        /// <summary>
        /// The Action equivalent of "results" in the Goal class.
        /// </summary>
        /// <param name="goal"></param>
        /// <param name="examinee"></param>
        /// <returns></returns>
        public override (float, BaseAction, ColonistState) PredictFit(Func<ColonistState, WorldObjectInfo, float> predicate, ColonistState examinee)
        {
            //TODO: Update sleep time to be derived from GameTime whenever it is implemented.
            float sleepTime = 200;

            examinee.needs += (benefit * sleepTime);

            return (predicate(examinee, WorldObjectInfo.none), new TherapyAction(this.seat), examinee);
        }
    }
}
