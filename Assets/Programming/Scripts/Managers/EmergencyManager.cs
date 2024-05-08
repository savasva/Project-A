using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Linq;

public class EmergencyManager : MonoBehaviour
{
    public static EmergencyManager inst;

    [Header("State")]
    [SerializeField] int dayLength = 300;
    [SerializeField] float time = 0f;
    [SerializeField] float currentIntensity;
    [SerializeField] bool activeEmergency = false;

    [Header("Aesthetics")]
    [SerializeField] Color emergencyColor;

    public DirectorCurve storyCurve;

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

    private void Update()
    {
        currentIntensity = storyCurve.curve.Evaluate(Mathf.Clamp01(time / dayLength));

        time += Time.deltaTime;
    }

    [Button]
    void StartFire()
    {
        FireEmergency test = new FireEmergency(currentIntensity);
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
