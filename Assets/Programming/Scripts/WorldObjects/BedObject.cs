using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedObject : MonoBehaviour
{
    public Needs benefit = new Needs(0, 0, 0.05f);

    public class SleepAction : BaseAction
    {
        BedObject bed;

        public SleepAction(): base() { }

        public override (float, BaseAction) PredictFit(Goal goal, ColonistState examinee)
        {
            //TODO: Update sleep time to be derived from GameTime whenever it is implemented.
            float sleepTime = 200;

            examinee.needs += (benefit * sleepTime);

            return (goal.postconditionFit(examinee), new SleepAction());
        }
    }
}
