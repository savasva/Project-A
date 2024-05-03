using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager inst;

    [SerializeField] GameObject Sidebar;
    [SerializeField] GameObject ButtonTemplate;

    [Header("Camera Screen")]
    //Screen GameObjects
    [SerializeField] GameObject CameraScreen;

    [Header("Alert Screen")]
    [SerializeField] GameObject AlertScreen;

    [Header("Chat Screen")]
    [SerializeField] GameObject ChatScreen;
    [SerializeField] GameObject ChatPanelTemplate;
    
    Dictionary<Colonist, ChatPanel> ChatPanels = new();

    [Header("Profile Screen")]
    [SerializeField] GameObject ProfilesScreen;

    [Header("Settings Screen")]
    [SerializeField] GameObject SettingsScreen;

    private void Awake()
    {
        if (inst == null)
            inst = this;
        else if (inst != this)
            Destroy(this);
    }

    void Start()
    {
        CreateChatPanels();
        SwitchToCameraScreen();
    }

    public void SwitchToCameraScreen()
    {
        PopulateCameraSidebar();

        //deactivate all screens except the camera screen
        AlertScreen.SetActive(false);
        ChatScreen.SetActive(false);
        ProfilesScreen.SetActive(false);
        SettingsScreen.SetActive(false);

        //activate the camera screen
        CameraScreen.SetActive(true);
    }

    void PopulateCameraSidebar()
    {
        ClearSidebar();

        foreach (CameraObject obj in ColonyManager.inst.cameraObjects)
        {
            GameObject btnObj = Instantiate(ButtonTemplate, Sidebar.transform, false);
            Button btn = btnObj.GetComponent<Button>();

            btnObj.GetComponentInChildren<TMP_Text>().text = obj.info.name;

            btn.onClick.AddListener(() =>
            {
                CameraManager.inst.UpdateCurrentCam(obj);
            });
        }
    }

    void ClearSidebar()
    {
        foreach(Transform t in Sidebar.transform)
        {
            Destroy(t.gameObject);
        }
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
        PopulateChatSidebar();

        //deactivate all screens except the chat screen
        CameraScreen.SetActive(false);
        AlertScreen.SetActive(false);
        ProfilesScreen.SetActive(false);
        SettingsScreen.SetActive(false);

        //activate the chat screen
        ChatScreen.SetActive(true);
    }

    void PopulateChatSidebar()
    {
        ClearSidebar();

        foreach (Colonist col in ColonyManager.inst.colonists.Values)
        {
            GameObject btnObj = Instantiate(ButtonTemplate, Sidebar.transform, false);
            Button btn = btnObj.GetComponent<Button>();

            btnObj.GetComponentInChildren<TMP_Text>().text = col.model.name;

            btn.onClick.AddListener(() =>
            {
                SelectChat(col);
            });
        }
    }

    public void SelectChat(Colonist col)
    {
        foreach (KeyValuePair<Colonist, ChatPanel> panel in ChatPanels)
        {
            if (panel.Key == col)
                panel.Value.On();
            else
                panel.Value.Off();
        }
    }

    void CreateChatPanels()
    {
        foreach (Colonist col in ColonyManager.inst.colonists.Values)
        {
            ChatPanel panel = Instantiate(ChatPanelTemplate, ChatScreen.transform, false).GetComponent<ChatPanel>();
            panel.Init(col);
            panel.Off();
            ChatPanels.Add(col, panel);
        }
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
