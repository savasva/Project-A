using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PromptDisplay : MonoBehaviour
{
    public CAINManager.CainQuestion question;
    public Text questionText;
    public Button yes;
    public Button no;

    public void Start()
    {
        yes.onClick.AddListener(() =>
        {
            Respond("yes");
        });

        no.onClick.AddListener(() =>
        {
            Respond("no");
        });
    }

    public void Respond(string response)
    {
        bool approved = (response.ToLower() == "yes");
        //question.caller.ReceiveResponse(approved);
        Destroy(yes.gameObject);
        Destroy(no.gameObject);
    }
}
