using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CAINManager : MonoBehaviour
{
    public static CAINManager inst;

    //UI functions exist here because I don't want to add even more scripts. Should be exported to UIManager ASAP.
    public Transform terminalTextScroll;
    public GameObject terminalEntryTemplate;

    private void Awake()
    {
        if (inst == null)
            inst = this;
        else if (inst != this)
            Destroy(this);
    }

    private void Start()
    {

    }

    public void AskCain(CainQuestion question)
    {
        GameObject prompt = Instantiate(terminalEntryTemplate, terminalTextScroll);
        PromptDisplay display = prompt.GetComponent<PromptDisplay>();
        display.questionText.text = question.prompt;
        display.question = question;
    }

    public class CainQuestion
    {
        public RequestWorldModGoal caller;
        public string prompt;

        public CainQuestion(string _prompt, RequestWorldModGoal _caller)
        {
            prompt = _prompt;
            caller = _caller;
        }
    }
}
