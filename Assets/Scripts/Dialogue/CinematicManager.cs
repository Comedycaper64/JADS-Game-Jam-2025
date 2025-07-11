using System;
using System.Collections.Generic;
using UnityEngine;

public class CinematicManager : MonoBehaviour
{
    private ConversationNode currentCinematicNode;
    private Queue<ConversationNode> cinematicNodes;
    private Action OnCinematicFinished;

    [SerializeField]
    private DialogueManager dialogueManager;

    [SerializeField]
    private ConversationSO testConversation;

    public static CinematicManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            // Debug.LogError(
            //     "There's more than one CinematicManager! " + transform + " - " + Instance
            // );
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        //TESTING
        PlayCinematic(testConversation, null);
    }

    public void PlayCinematic(ConversationSO conversationSO, Action OnCinematicFinished)
    {
        this.OnCinematicFinished = OnCinematicFinished;
        cinematicNodes = new Queue<ConversationNode>(conversationSO.GetConversationNodes());
        TryPlayNextNode();
    }

    private void TryPlayNextNode()
    {
        if (!cinematicNodes.TryDequeue(out currentCinematicNode))
        {
            EndCinematic();
            return;
        }

        Type nodeType = currentCinematicNode.GetType();

        //Debug.Log("Playing Node: " + nodeType.ToString());

        if (nodeType == typeof(DialogueSO))
        {
            dialogueManager.PlayDialogue(currentCinematicNode as DialogueSO, TryPlayNextNode);
        }
        else if (nodeType == typeof(PlaySFXSO))
        {
            PlaySFXSO sfx = currentCinematicNode as PlaySFXSO;

            AudioManager.PlaySFX(
                sfx.soundEffect,
                sfx.sfxVolume,
                sfx.sfxPitch,
                Camera.main.transform.position
            );

            TryPlayNextNode();
        }
        else if (nodeType == typeof(ScreenEffectSO))
        {
            ScreenEffectSO screenEffect = currentCinematicNode as ScreenEffectSO;
            screenEffect.onEffectComplete = TryPlayNextNode;

            //ScreenEffectManager.PlayEffect
        }
        else if (nodeType == typeof(ClueSO))
        {
            ClueSO clue = currentCinematicNode as ClueSO;

            //Add Clue to clue manager
        }
        else
        {
            //Debug.Log("Error, undefined type");
        }
    }

    private void EndCinematic()
    {
        currentCinematicNode = null;
        OnCinematicFinished?.Invoke();
    }
}
