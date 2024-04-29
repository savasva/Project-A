using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    //Screen GameObjects
    [SerializeField] private GameObject CameraScreen;
    [SerializeField] private GameObject AlertScreen;
    [SerializeField] private GameObject ChatScreen;
    [SerializeField] private GameObject ProfilesScreen;
    [SerializeField] private GameObject SettingsScreen;

    void Awake()
    {
        SwitchToCameraScreen();
    }

    public void SwitchToCameraScreen()
    {
        //deactivate all screens except the camera screen
        AlertScreen.SetActive(false);
        ChatScreen.SetActive(false);
        ProfilesScreen.SetActive(false);
        SettingsScreen.SetActive(false);

        //activate the camera screen
        CameraScreen.SetActive(true);
    }

    public void SwitchToAlertScreen()
    {
        //deactivate all screens except the alert screen
        CameraScreen.SetActive(false);
        ChatScreen.SetActive(false);
        ProfilesScreen.SetActive(false);
        SettingsScreen.SetActive(false);

        //activate the alert screen
        AlertScreen.SetActive(true);
    }

    public void SwitchToChatScreen()
    {
        //deactivate all screens except the chat screen
        CameraScreen.SetActive(false);
        AlertScreen.SetActive(false);
        ProfilesScreen.SetActive(false);
        SettingsScreen.SetActive(false);

        //activate the chat screen
        ChatScreen.SetActive(true);
    }

    public void SwitchToProfilesScreen()
    {
        //deactivate all screens except the profiles screen
        CameraScreen.SetActive(false);
        AlertScreen.SetActive(false);
        ChatScreen.SetActive(false);
        SettingsScreen.SetActive(false);

        //activate the profiles screen
        ProfilesScreen.SetActive(true);
    }

    public void SwitchToSettingsScreen()
    {
        //deactivate all screens except the settings screen
        CameraScreen.SetActive(false);
        AlertScreen.SetActive(false);
        ChatScreen.SetActive(false);
        ProfilesScreen.SetActive(false);

        //activate the profiles screen
        SettingsScreen.SetActive(true);
    }
}
