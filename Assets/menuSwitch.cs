using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuSwitch : MonoBehaviour
{
    [SerializeField] private bool chatSelected = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void chatButtonPressed() {
        chatSelected = true;
    }

    public void fileButtonPressed() {
        chatSelected = false;
    }
}
