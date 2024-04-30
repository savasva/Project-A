using UnityEngine;
using System.Collections;
using TMPro;

public class AsyncChatEntry : MonoBehaviour
{
	[SerializeField]
	TMP_Text textbox;

	public void Append(string addition) {
		textbox.text += addition;
	}
}

