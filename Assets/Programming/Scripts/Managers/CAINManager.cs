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

        DontDestroyOnLoad(this);
    }

    private void Start()
    {

    }

    public void AskCain(CainQuestion question)
    {
        /*
        GameObject prompt = Instantiate(terminalEntryTemplate, terminalTextScroll);
        PromptDisplay display = prompt.GetComponent<PromptDisplay>();
        display.questionText.text = question.prompt;
        display.question = question;
        */

        QuickAIDialogue.singleton.AddQuestion(question);
    }

    [System.Serializable]
    public class CainQuestion
    {
        public WorldModGoal caller;
        public string prompt;

        public CainQuestion(string _prompt, WorldModGoal _caller)
        {
            prompt = _prompt;
            caller = _caller;
        }
    }
}
