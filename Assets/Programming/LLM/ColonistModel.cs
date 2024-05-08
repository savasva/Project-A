using UnityEngine;
using LLMUnity;

[RequireComponent(typeof(LLMClient))]
public class ColonistModel : MonoBehaviour
{
    public new string name;
    public string lastAnswer;

    [Header("Prompt Parameters")]
    [TextArea(3, 10)]
    public string prompt;
    public int maxTokens = 64;
    [Range(0, 1f)]
    public float temperature = 0.6f;

    public LLMClient llm;

    [Header("UI Parameters")]
    public GameObject chatMessageContainer;
}