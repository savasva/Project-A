using UnityEngine;
using System.Collections;
using TMPro;

public class AsyncChatEntry : MonoBehaviour
{
	[SerializeField]
	TMP_Text textbox;
	string goalString = "";
	bool fullResponse = false;

	public void Append(string addition) {
        //textbox.text += addition;

        goalString += addition;


        StartCoroutine(CharAppend(addition));
	}

	public void MarkComplete()
	{
		fullResponse = true;
	}

	IEnumerator CharAppend(string addition)
	{
		const float delay = 0.25f;

		while (!fullResponse) {
			if (textbox.text.Length < goalString.Length)
			textbox.text += goalString[textbox.text.Length];
			yield return new WaitForSeconds(delay);
		}
	}
}

