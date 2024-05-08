using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusButton : MonoBehaviour
{
    [SerializeField] WorldObject obj;
    [SerializeField] TMP_Text condText;

    public void Init(WorldObject target)
    {
        obj = target;
        condText = GetComponentInChildren<TMP_Text>(true);
    }

    public void Draw(bool composite = true)
    {
        string statusText = string.Format("<b>{0}</b>\n", obj.info.name);

        DamagableProperty prop = obj.info.GetProperty<DamagableProperty>();

        if (composite)
        {
            float durability = 0f;
            foreach (WorldObjComponent comp in prop.components)
            {
                durability += comp.durability;
            }
            durability /= prop.components.Count;

            statusText += string.Format("Durability: {0}%\n", (durability * 100).ToString("N2"));
        }
        else
        {
            foreach (WorldObjComponent comp in prop.components)
            {
                statusText += string.Format("{0}: {1}%\n", comp.name, (comp.durability * 100).ToString("N2"));
            }
        }
        

        condText.text = statusText;
    }
}
