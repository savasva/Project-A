using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Linq;

public class EmergencyManager : MonoBehaviour
{
    public static EmergencyManager inst;

    [SerializeField]
    Color emergencyColor;

    [SerializeField]
    bool activeEmergency = false;

    [SerializeField]
    [Range(0, 1f)]
    float fireIntensity;

    private void Awake()
    {
        if (inst == null)
            inst = this;
        else if (inst != this)
            Destroy(this);
    }

    private void Start()
    {
        StartCoroutine(EmergencyLightLoop(2f));
    }

    [Button]
    void StartFire()
    {
        FireEmergency test = new FireEmergency(fireIntensity);
        //test.targets.ForEach(t => Debug.Log(t.name));
        StartEmergency(test);
    }

    public void StartEmergency(Emergency emergency)
    {
        activeEmergency = true;
        emergency.Execute();
    }

    IEnumerator EmergencyLightLoop(float delay)
    {
        bool toggle = false;

        List<EmergencyLight> lights = ColonyManager.inst.lights.Select((Light l) => l.GetComponent<EmergencyLight>()).ToList();

        while (true)
        {
            if (!activeEmergency) 
            {
                if (toggle)
                {
                    foreach (EmergencyLight light in lights)
                    {
                        light.ResetColor();
                    }
                    toggle = false;
                }
                    
                yield return new WaitForEndOfFrame();
                continue;
            }

            foreach (EmergencyLight light in lights)
            {
                //Flash lights
                if (!toggle)
                {
                    light.SetColor(emergencyColor);
                }
                else
                {
                    light.ResetColor();
                }
            }

            toggle = !toggle;
            
            yield return new WaitForSeconds(delay);
        }
    }
}
