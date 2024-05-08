using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileManager : MonoBehaviour
{
    public static ProfileManager inst;
    [SerializeField] private GameObject EngrTextPanel;
    [SerializeField] private GameObject BioTextPanel;
   
    void Awake()
    {
        if (inst == null)
        {
            inst = this;
        }
        else if (inst != this)
        {
            Destroy(this);
        }
        
        SwitchToEngrProfile();
    }

    public void SwitchToEngrProfile()
    {
        //deactivate all profile views except the engr profile
        BioTextPanel.SetActive(false);

        //activate the engr profile 
        EngrTextPanel.SetActive(true);
    }

    public void SwitchToBioProfile()
    {
        //deactivate all profile views except the bio profile
        EngrTextPanel.SetActive(false);

        //activate the bio profile 
        BioTextPanel.SetActive(true);
    }
}
