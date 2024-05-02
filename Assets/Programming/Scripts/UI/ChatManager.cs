using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatManager : MonoBehaviour
{
    [SerializeField] private GameObject EngrChatScreen;
    [SerializeField] private GameObject BioChatScreen;

    void Awake()
    {
        SwitchToEngrChat();
    }

    public void SwitchToEngrChat()
    {
        //deactivate all chat screen views except the engr chat
        BioChatScreen.SetActive(false);

        //activate the engr chat 
        EngrChatScreen.SetActive(true);
    }

    public void SwitchToBioChat()
    {
        //deactivate all chat screen views except the bio chat
        EngrChatScreen.SetActive(false);

        //activate the bio chat 
        BioChatScreen.SetActive(true);
    }
}
