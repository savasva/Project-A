using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ASK : Planbox
{
    RequestMemory question;
    Colonist recipient;

    public ASK(Colonist _doer, RequestMemory _question) : base(_doer)
    {
        question = _question;
    }

    /*public async override UniTask<bool> Act()
    {
        MTRANS mtrans = new MTRANS(doer, "Ask {0} for {1}.", Random.Range(0f, 10000f).ToString(), question, recipient.stm);
        

        return true;
    }*/
}
