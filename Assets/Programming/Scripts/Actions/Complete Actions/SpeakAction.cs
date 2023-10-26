using UnityEngine;
using System.Collections;

public class SpeakAction : BaseAction
{
    public Colonist conversant;

    public SpeakAction(Colonist _conversant)
    {
        conversant = _conversant;
        name = string.Format("Talk to {0}", conversant.role);
    }
}
