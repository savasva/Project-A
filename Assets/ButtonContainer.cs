using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonContainer : MonoBehaviour
{
    [SerializeField]
    GameObject container;

    [SerializeField]
    TMP_Text title;

    [SerializeField]
    Image arrow;

    [SerializeField]
    Button button;
    bool toggle = true;

    public void Init(string _title)
    {
        title.text = _title;

        button.onClick.AddListener(() =>
        {
            Toggle();
        });
    }

    public void AddButton(GameObject child)
    {
        child.transform.SetParent(container.transform);
    }

    public void Toggle()
    {
        toggle = !toggle;

        container.SetActive(toggle);

        float rotation = 90;
        if (toggle)
        {
            rotation = -90;
        }
        arrow.transform.Rotate(new Vector3(0, 0, rotation));
    }
}
