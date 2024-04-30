using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Linq;

public class ChatManager : MonoBehaviour
{
    //Message Text Field
    [SerializeField] private TMP_Text MessageTextComponent;

    //[SerializeField] private GameObject recalTextObject;
    private string txt;

    void Start()
    {
        //get engr chat log
        //...

        //get bio chat log
        //...

        //store all lines of file in fileLines
        //string fileLines = File.ReadAllText(txt);
        //List<string> fileLines = File.ReadAllLines(readFromFilePath).ToList();

        //
    }

    public void SelectEngrChat()
    {
        //set the text shown in the text component to the engineer's chat history
        //MessageTextComponent.text = ;
    }

    public void SelectBioChat()
    {

    }
}
