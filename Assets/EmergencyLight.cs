using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyLight : MonoBehaviour
{
    Color startColor;
    Light light;

    private void Start()
    {
        light = GetComponent<Light>();
        startColor = light.color;
    }

    public void SetColor(Color color)
    {
        light.color = color;
    }

    public void ResetColor()
    {
        light.color = startColor;
    }
}
