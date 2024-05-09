using UnityEngine;
using System.Collections;
using TMPro;

public class AsyncChatEntry : MonoBehaviour
{
	[SerializeField]
	TMP_Text textbox;
	string goalString = "";
	bool started = false;
	bool fullResponse = false;

	public void Append(string addition) {
        //textbox.text += addition;

        goalString += addition;
	}

	public void Set(string text)
	{
		goalString = text;
		if (!started) ColonyManager.inst.StartCoroutine(CharAppend());
    }

    public void MarkComplete()
	{
		fullResponse = true;
	}

	IEnumerator CharAppend()
	{
		const float delay = 0.05f;

		started = true;
		textbox.text = "";

		while (!fullResponse) {
			if (textbox.text.Length < goalString.Length)
			textbox.text += goalString[textbox.text.Length];
			yield return new WaitForSeconds(delay);
		}
	}
}

