using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedObject : WorldObject
{
    public class SleepAction : BaseAction
    {
        [SerializeField]
        BedObject bed;

        public override Func<ColonistState, bool> precondition
        {
            get => (ColonistState state) => {
                return (Vector3.Distance(state.position, bed.moveDestination.position) <= 2.5f);
            };
        }

        public SleepAction(BedObject _bed): base() {
            bed = _bed;
        }

        public override void OnTick()
        {
            base.OnTick();

            doer.state.needs += bed.benefit * Time.deltaTime;

            if (doer.state.needs.tiredness <= -1f)
            {
                Debug.Log("Complete!");
                doer.mover.ResetPath();
                Complete();
            }
        }

        /// <summary>
        /// The Action equivalent of "results" in the Goal class.
        /// </summary>
        /// <param name="goal"></param>
        /// <param name="examinee"></param>
        /// <returns></returns>
        public override (float, BaseAction, ColonistState) PredictFit(Goal goal, ColonistState examinee)
        {
            //TODO: Update sleep time to be derived from GameTime whenever it is implemented.
            float sleepTime = 200;

            examinee.needs += (benefit * sleepTime);

            return (goal.resultFit(examinee), new SleepAction(this.bed), examinee);
        }


        public override (float, BaseAction, ColonistState) PredictFit(BaseAction prevAction, ColonistState examinee)
        {
            //TODO: Update sleep time to be derived from GameTime whenever it is implemented.
            float sleepTime = 200;

            examinee.needs += (benefit * sleepTime);

            return (prevAction.precondition(examinee) ? 1f : 0f, new SleepAction(this.bed), examinee);
        }
    }
}
