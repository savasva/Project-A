using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Terminal : MonoBehaviour
{
    [SerializeField]
    Image bg;

    [SerializeField]
    TMP_TextElement textElement;

    Queue<string[]> terminal = new Queue<string[]>();

    public Terminal(TMP_TextElement _textEl, string _input)
    {
        textElement = _textEl;
    }

    void PrintLine()
    {

    }

    void Print()
    {

    }

    void Parse()
    {

    }
}
