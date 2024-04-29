using LLama;
using LLama.Common;
using UnityEngine;

[CreateAssetMenu(fileName = "New LLM Model", menuName = "Project A/LLM Model")]
[System.Serializable]
public class ColonistModel : ScriptableObject
{
    public new string name;
    public string lastAnswer;
    [TextArea(3, 10)]
    public string prompt;

    public ChatHistory chatHistory;

    public ChatSession session;
}