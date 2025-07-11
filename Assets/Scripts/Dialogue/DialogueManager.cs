using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DialogueUIEventArgs
{
    public DialogueUIEventArgs(
        ActorSO actorSO,
        string sentence,
        int dialogueSpriteIndex,
        Action onTypingFinished
    )
    {
        this.actorSO = actorSO;
        this.sentence = sentence;
        this.dialogueSpriteIndex = dialogueSpriteIndex;
        this.onTypingFinished = onTypingFinished;
    }

    public ActorSO actorSO;
    public string sentence;
    public int dialogueSpriteIndex;
    public Action onTypingFinished;
}

public class DialogueManager : MonoBehaviour
{
    //private bool bLogActive = false;
    private bool bIsSentenceTyping;
    private bool bLoopToChoice;

    //private Coroutine autoPlayCoroutine;

    private ActorSO currentActor;

    //private DialogueChoiceSO currentChoice;
    private string currentSentence;
    private int currentSpriteIndex;
    private Queue<string> currentDialogue;
    private Queue<DialogueSprite> currentSprites;
    private Action onDialogueComplete;
    private Queue<Dialogue> dialogues;
    public static event Action OnFinishTypingDialogue;
    public static event EventHandler<bool> OnToggleDialogueUI;

    //public static event EventHandler<DialogueChoiceUIEventArgs> OnDisplayChoices;

    //public static event EventHandler<Sprite[]> OnChangeSprite;
    public static event EventHandler<DialogueUIEventArgs> OnDialogue;

    private void Awake()
    {
        // DialogueAutoPlayUI.OnAutoPlayToggle += ToggleAutoPlay;
        // DialogueLogUI.OnLogToggle += OnLogToggle;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnAdvanceAction -= InputManager_OnAdvance;

        // if (autoPlayCoroutine != null)
        // {
        //     StopCoroutine(autoPlayCoroutine);
        // }
    }

    public void PlayDialogue(DialogueSO dialogueSO, Action onDialogueComplete)
    {
        this.onDialogueComplete = onDialogueComplete;
        dialogues = new Queue<Dialogue>(dialogueSO.GetDialogues());
        InputManager.Instance.OnAdvanceAction += InputManager_OnAdvance;

        ToggleDialogueUI(true);
        TryPlayNextDialogue();
    }

    // public void DisplayChoices(DialogueChoiceSO dialogueChoiceSO, Action onDialogueComplete)
    // {
    //     this.onDialogueComplete = onDialogueComplete;
    //     currentChoice = dialogueChoiceSO;
    //     ToggleDialogueUI(true);
    //     DialogueChoiceUIEventArgs choiceUIEventArgs = new DialogueChoiceUIEventArgs(
    //         dialogueChoiceSO,
    //         PlayChoiceDialogue
    //     );
    //     OnDisplayChoices?.Invoke(this, choiceUIEventArgs);
    // }

    // private void PlayChoiceDialogue(object sender, DialogueChoice dialogueChoice)
    // {
    //     if (dialogueChoice.loopBackToChoice)
    //     {
    //         bLoopToChoice = true;
    //     }

    //     dialogues = new Queue<Dialogue>(
    //         currentChoice.GetDialogueAnswers()[dialogueChoice.correspondingDialogue].dialogueAnswers
    //     );
    //     InputManager.Instance.OnShootAction += InputManager_OnShootAction;

    //     TryPlayNextDialogue();
    // }

    private void TryPlayNextDialogue()
    {
        if (!dialogues.TryDequeue(out Dialogue dialogueNode))
        {
            EndDialogue();
            return;
        }
        currentActor = dialogueNode.actor;

        currentSprites = new Queue<DialogueSprite>(dialogueNode.dialogueSprites);

        currentDialogue = new Queue<string>(dialogueNode.dialogue);

        DisplayNextSentence();
    }

    private void DisplayNextSentence()
    {
        //Debug.Log("Go, go go!");

        if (bIsSentenceTyping)
        {
            //Debug.Log("We're typin here!");
            OnFinishTypingDialogue?.Invoke();
            return;
        }

        //Debug.Log("Next sentence");

        // if (autoPlayCoroutine != null)
        // {
        //     StopCoroutine(autoPlayCoroutine);
        // }


        if (!currentDialogue.TryDequeue(out currentSentence))
        {
            TryPlayNextDialogue();
            return;
        }

        if (currentSprites.TryDequeue(out DialogueSprite newDialogueSprite))
        {
            currentSpriteIndex = (int)newDialogueSprite;
        }
        else
        {
            currentSpriteIndex = -1;
        }

        if (currentSentence == "")
        {
            ToggleDialogueUI(false);

            // if (animationTimer == 0f)
            // {
            //     animationCoroutine = StartCoroutine(AnimationPause(animationTimer));
            // }
        }
        else
        {
            ToggleDialogueUI(true);
            StartTypingSentence();
        }
    }

    private IEnumerator DialogueAutoPlayTimer(float dialogueTime)
    {
        yield return new WaitForSeconds(dialogueTime);
        DisplayNextSentence();
    }

    private void StartTypingSentence()
    {
        bIsSentenceTyping = true;

        OnDialogue?.Invoke(
            this,
            new DialogueUIEventArgs(
                currentActor,
                currentSentence,
                currentSpriteIndex,
                FinishTypingSentence
            )
        );
    }

    private void FinishTypingSentence()
    {
        bIsSentenceTyping = false;
    }

    private void ToggleDialogueUI(bool toggle)
    {
        OnToggleDialogueUI?.Invoke(this, toggle);
    }

    private void ResetChoices()
    {
        // DialogueChoiceUIEventArgs blankChoiceUIEventArgs = new DialogueChoiceUIEventArgs(
        //     null,
        //     null
        // );
        // OnDisplayChoices?.Invoke(this, blankChoiceUIEventArgs);
    }

    private void EndDialogue(bool skipping = false)
    {
        InputManager.Instance.OnAdvanceAction -= InputManager_OnAdvance;

        ToggleDialogueUI(false);

        if (bLoopToChoice)
        {
            // bLoopToChoice = false;
            // DisplayChoices(currentChoice, onDialogueComplete);
        }
        else
        {
            ResetChoices();

            if (!skipping)
            {
                onDialogueComplete();
            }
            else
            {
                onDialogueComplete = null;
            }
        }
    }

    private void InputManager_OnAdvance()
    {
        // if (bLogActive)
        // {
        //     return;
        // }

        DisplayNextSentence();
    }
}
