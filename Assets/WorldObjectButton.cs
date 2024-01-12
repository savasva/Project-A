using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorldObjectButton : MonoBehaviour
{
    WorldObject obj;

    public void Initialize(WorldObject target)
    {
        obj = target;
        transform.Find("Title").GetComponent<TextMeshProUGUI>().text = obj.info.name;
        GetComponent<Button>().onClick.AddListener(obj.OnUse);
    }

    private void OnClick()
    {
        
    }
}
