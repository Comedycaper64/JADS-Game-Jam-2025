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
    private bool bLogActive = false;
    private bool bIsSentenceTyping;
    private bool bIsSkipping = false;
    private float skipTimeBetweenAdvance = 0.1f;
    private ActorSO currentActor;
    private string currentSentence;
    private int currentSpriteIndex;
    private Queue<string> currentDialogue;
    private Queue<DialogueSprite> currentSprites;
    private Action onDialogueComplete;
    private Queue<Dialogue> dialogues;
    public static event Action OnFinishTypingDialogue;
    public static event EventHandler<bool> OnToggleDialogueUI;

    public static event EventHandler<DialogueUIEventArgs> OnDialogue;

    private void Awake()
    {
        DialogueLogUI.OnLogToggle += OnLogToggle;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnAdvanceAction -= InputManager_OnAdvance;
        InputManager.Instance.OnSkipStartAction -= InputManager_OnSkipStart;
        InputManager.Instance.OnSkipEndAction -= InputManager_OnSkipEndStart;
        DialogueLogUI.OnLogToggle -= OnLogToggle;
    }

    public void PlayDialogue(DialogueSO dialogueSO, Action onDialogueComplete)
    {
        this.onDialogueComplete = onDialogueComplete;
        dialogues = new Queue<Dialogue>(dialogueSO.GetDialogues());
        InputManager.Instance.OnAdvanceAction += InputManager_OnAdvance;
        InputManager.Instance.OnSkipStartAction += InputManager_OnSkipStart;
        InputManager.Instance.OnSkipEndAction += InputManager_OnSkipEndStart;

        ToggleDialogueUI(true);
        TryPlayNextDialogue();
    }

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
        if (bIsSentenceTyping)
        {
            OnFinishTypingDialogue?.Invoke();
            return;
        }

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
        }
        else
        {
            ToggleDialogueUI(true);
            StartTypingSentence();
        }
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

    private IEnumerator SkipDialogue()
    {
        if (bIsSkipping)
        {
            InputManager_OnAdvance();

            yield return new WaitForSeconds(skipTimeBetweenAdvance);

            StartCoroutine(SkipDialogue());
        }
    }

    private void EndDialogue(bool skipping = false)
    {
        InputManager.Instance.OnAdvanceAction -= InputManager_OnAdvance;
        InputManager.Instance.OnSkipStartAction -= InputManager_OnSkipStart;
        InputManager.Instance.OnSkipEndAction -= InputManager_OnSkipEndStart;

        bIsSkipping = false;

        ToggleDialogueUI(false);

        if (!skipping)
        {
            onDialogueComplete();
        }
        else
        {
            onDialogueComplete = null;
        }
    }

    private void InputManager_OnAdvance()
    {
        if (bLogActive)
        {
            return;
        }

        if (ClueManagerUI.inventoryOpen)
        {
            return;
        }

        DisplayNextSentence();
    }

    private void InputManager_OnSkipEndStart()
    {
        bIsSkipping = false;
    }

    private void InputManager_OnSkipStart()
    {
        bIsSkipping = true;
        StartCoroutine(SkipDialogue());
    }

    private void OnLogToggle(object sender, bool toggle)
    {
        bLogActive = toggle;
    }
}
