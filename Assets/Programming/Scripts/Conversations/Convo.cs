using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Convo
{
    [SerializeField]
    Author initiator;

    [SerializeField]
    Author target;

    List<Message> messages;

    public void Progress()
    {

    }

    public class Message
    {
        public Author author;
        public string contents;
    }
    public enum Author
    {
        Cain,
        Crewmate
    }
}
