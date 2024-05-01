using LLama;
using LLama.Common;
using UnityEngine;

[CreateAssetMenu(fileName = "New LLM Model", menuName = "Project A/LLM Model")]
[System.Serializable]
public class ColonistModel : ScriptableObject
{
    public new string name;
    public string lastAnswer;

    [Header("Prompt Parameters")]
    [TextArea(3, 10)]
    public string prompt;
    public int maxTokens = 64;
    [Range(0, 1f)]
    public float temperature = 0.6f;

    public ChatHistory chatHistory;

    public ChatSession session;

    [Header("UI Parameters")]
    public GameObject chatMessageContainer;
}