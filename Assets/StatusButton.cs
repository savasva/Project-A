using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusButton : MonoBehaviour
{
    [SerializeField] WorldObject obj;

    public void Init(WorldObject target)
    {
        obj = target;
    }

    public void Draw()
    {
        string statusText = string.Format("<b>{0}</b>\n", obj.info.name);

        DamagableProperty prop = obj.info.GetProperty<DamagableProperty>();

        foreach (WorldObjComponent comp in prop.components)
        {
            statusText += string.Format("{0} - {1}%\n", comp.name, comp.durability * 100);
        }

        obj.GetComponentInChildren<TMP_Text>().text = statusText;
    }
}
