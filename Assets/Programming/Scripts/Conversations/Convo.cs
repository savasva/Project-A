using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Convo : MonoBehaviour
{
    [SerializeField]
    string systemPrompt;

    public class Message
    {
        public Author author;
        public string contents;

        public enum Author
        {
            Cain,
            Crewmate
        }
    }
}
