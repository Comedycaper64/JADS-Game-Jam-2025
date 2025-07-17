using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Random = UnityEngine.Random;

public class DialogueUI : MonoBehaviour
{
    private bool bIsDialogueActive = false;
    private bool isTyping = false;
    private bool bDialogueSpriteActive = false;
    private bool bPlayDialogueNoises = true;

    private int charsBetweenDialogueNoises = 3;
    private float timeBetweenLetterTyping = 0.025f;

    private const float LOW_PITCH_RANGE = 0.75f;
    private const float HIGH_PITCH_RANGE = 1.25f;

    //private string typingSentence;
    private Coroutine typingCoroutine;
    private Coroutine actorFadeInCoroutine;
    private Action onTypingFinished;

    private CanvasGroupFader dialogueFader;

    [SerializeField]
    private CanvasGroupFader actorSpriteFader;

    [SerializeField]
    private Animator actorSpriteAnimator;

    [SerializeField]
    private Image dialogueFaceSprite;

    private ActorSO currentActor;
    private ActorSO lastVisibleActor;
    private AudioClip currentDialogueNoise;
    private AudioSource dialogueNoiseSource;

    [SerializeField]
    private TextMeshProUGUI actorNameText;

    [SerializeField]
    private Image actorNameFlourish;
    private Material actorFontMaterialInstance;

    [SerializeField]
    private TextMeshProUGUI dialogueText;

    [SerializeField]
    private Image dialogueTextbox;

    public EventHandler<DialogueLog> OnNewDialogue;

    private void Awake()
    {
        DialogueManager.OnToggleDialogueUI += DialogueManager_OnToggleDialogueUI;
        DialogueManager.OnDialogue += DialogueManager_OnDialogue;
        DialogueManager.OnFinishTypingDialogue += DialogueManager_OnFinishTypingDialogue;

        ClearDialogueText();
        actorFontMaterialInstance = actorNameText.fontMaterial;
        SetActorName("", Color.black);
        dialogueFader = GetComponent<CanvasGroupFader>();
        dialogueFader.SetCanvasGroupAlpha(0f);
        dialogueFader.ToggleBlockRaycasts(false);
        actorSpriteFader.SetCanvasGroupAlpha(0f);
        dialogueNoiseSource = GetComponent<AudioSource>();
    }

    private void OnDisable()
    {
        DialogueManager.OnToggleDialogueUI -= DialogueManager_OnToggleDialogueUI;
        DialogueManager.OnDialogue -= DialogueManager_OnDialogue;
        DialogueManager.OnFinishTypingDialogue -= DialogueManager_OnFinishTypingDialogue;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        if (actorFadeInCoroutine != null)
        {
            StopCoroutine(actorFadeInCoroutine);
        }
    }

    private void SetActorName(string actorName, Color nameColour)
    {
        actorNameText.text = actorName;

        actorNameFlourish.color = nameColour;

        dialogueTextbox.color = nameColour;
        dialogueTextbox.color = new Color(
            dialogueTextbox.color.r,
            dialogueTextbox.color.g,
            dialogueTextbox.color.b,
            0.98f
        );

        actorFontMaterialInstance.SetColor("_UnderlayColor", nameColour);
    }

    private void ClearDialogueText()
    {
        dialogueText.text = "";
    }

    private void ToggleDialogueActive(bool toggle)
    {
        bIsDialogueActive = toggle;
        dialogueFader.ToggleFade(toggle);
        dialogueFader.ToggleBlockRaycasts(toggle);

        if (!toggle)
        {
            if (actorFadeInCoroutine != null)
            {
                StopCoroutine(actorFadeInCoroutine);
            }
        }
    }

    private void SetNewActor(ActorSO actorSO)
    {
        SetActorName(actorSO.GetActorName(), actorSO.GetActorNameColour());

        if (currentActor == actorSO)
        {
            return;
        }

        currentActor = actorSO;

        currentDialogueNoise = actorSO.GetDialogueNoises();

        if (actorSO.GetDialogueSprites().Length > 0)
        {
            lastVisibleActor = actorSO;
        }
    }

    private IEnumerator TypeSentence(DialogueUIEventArgs dialogueUIEventArgs)
    {
        SetNewActor(dialogueUIEventArgs.actorSO);

        dialogueText.text = dialogueUIEventArgs.sentence;
        ResolveDialogueSprite(dialogueUIEventArgs);

        onTypingFinished = dialogueUIEventArgs.onTypingFinished;

        dialogueText.maxVisibleCharacters = 0;

        yield return null;

        string parsedText = dialogueText.GetParsedText();

        isTyping = true;

        int noiseTracker = 0;

        if (currentDialogueNoise != null)
        {
            dialogueNoiseSource.clip = currentDialogueNoise;
            dialogueNoiseSource.volume = PlayerOptions.GetVoiceVolume();
            bPlayDialogueNoises = true;
        }
        else
        {
            bPlayDialogueNoises = false;
        }

        for (int i = 0; i < parsedText.Length; i++)
        {
            dialogueText.maxVisibleCharacters++;

            if (bPlayDialogueNoises)
            {
                noiseTracker++;
                if (noiseTracker >= charsBetweenDialogueNoises)
                {
                    dialogueNoiseSource.Stop();
                    dialogueNoiseSource.pitch = Random.Range(LOW_PITCH_RANGE, HIGH_PITCH_RANGE);
                    dialogueNoiseSource.Play();
                    noiseTracker = 0;
                }
            }

            yield return new WaitForSeconds(timeBetweenLetterTyping);
        }

        isTyping = false;
        onTypingFinished();
    }

    private IEnumerator ChangeSprites(Sprite newSprite)
    {
        actorSpriteAnimator.SetTrigger("pulse");

        yield return new WaitForSeconds(0.1f);

        dialogueFaceSprite.sprite = newSprite;

        dialogueFaceSprite.SetNativeSize();
    }

    private void ResolveDialogueSprite(DialogueUIEventArgs dialogueUIEventArgs)
    {
        ActorSO currentActor = dialogueUIEventArgs.actorSO;
        if (currentActor.GetDialogueSprites().Length <= 0)
        {
            return;
        }

        int dialogueSpriteIndex = dialogueUIEventArgs.dialogueSpriteIndex;
        Sprite newSprite = null;

        if (dialogueSpriteIndex >= 0)
        {
            newSprite = currentActor.GetDialogueSprites()[dialogueUIEventArgs.dialogueSpriteIndex];
        }

        if (newSprite != null)
        {
            if (newSprite == dialogueFaceSprite.sprite)
            {
                return;
            }

            StartCoroutine(ChangeSprites(newSprite));

            if (!bDialogueSpriteActive)
            {
                actorSpriteFader.ToggleFade(true);
                dialogueFaceSprite.sprite = newSprite;
                bDialogueSpriteActive = true;
            }
        }
        else
        {
            actorSpriteFader.ToggleFade(false);
            bDialogueSpriteActive = false;
        }
    }

    private void DialogueManager_OnFinishTypingDialogue()
    {
        if (!isTyping)
        {
            return;
        }

        StopCoroutine(typingCoroutine);
        dialogueText.maxVisibleCharacters = dialogueText.GetParsedText().Length;
        onTypingFinished();
    }

    private void DialogueManager_OnDialogue(object sender, DialogueUIEventArgs dialogueArgs)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeSentence(dialogueArgs));

        OnNewDialogue?.Invoke(this, new DialogueLog(dialogueArgs.actorSO, dialogueArgs.sentence));
    }

    private void DialogueManager_OnToggleDialogueUI(object sender, bool toggle)
    {
        if (toggle == bIsDialogueActive)
        {
            return;
        }

        if (toggle)
        {
            dialogueFaceSprite.sprite = null;
        }

        ClearDialogueText();
        SetActorName("", Color.black);
        actorSpriteFader.ToggleFade(false);
        bDialogueSpriteActive = false;
        ToggleDialogueActive(toggle);
    }
}
