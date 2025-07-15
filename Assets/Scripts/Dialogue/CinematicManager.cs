using System;
using System.Collections.Generic;
using UnityEngine;

public class CinematicManager : MonoBehaviour
{
    private ConversationNode currentCinematicNode;
    private Queue<ConversationNode> cinematicNodes;
    private Action OnCinematicFinished;

    [SerializeField]
    private AudioManager audioManager;

    [SerializeField]
    private DialogueManager dialogueManager;

    [SerializeField]
    private ScreenEffectManager screenEffectManager;

    [SerializeField]
    private CluePopUpUI cluePopUpUI;

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

    public void PlayCinematic(ConversationSO conversationSO, Action OnCinematicFinished)
    {
        this.OnCinematicFinished = OnCinematicFinished;
        cinematicNodes = new Queue<ConversationNode>(conversationSO.GetConversationNodes());
        TryPlayNextNode();
    }

    private void TryPlayNextNode()
    {
        //Debug.Log("Play Next Node");

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
            screenEffectManager.PlayEffect(screenEffect);
        }
        else if (nodeType == typeof(ClueSO))
        {
            ClueSO clue = currentCinematicNode as ClueSO;

            ClueManager.Instance.AddClue(clue, TryPlayNextNode);
        }
        else if (nodeType == typeof(CluePresentSO))
        {
            CluePresentSO cluePresent = currentCinematicNode as CluePresentSO;

            ClueManager.Instance.StartCluePresent(cluePresent, OnCinematicFinished);
        }
        else if (nodeType == typeof(ChangeMusicSO))
        {
            ChangeMusicSO musicChange = currentCinematicNode as ChangeMusicSO;

            if (musicChange.newMusicTrack)
            {
                audioManager.ChangeMusicTrack(musicChange.newMusicTrack);
            }
            else
            {
                audioManager.StopMusic();
            }

            TryPlayNextNode();
        }
        else if (nodeType == typeof(CluePopUpSO))
        {
            CluePopUpSO cluePopUp = currentCinematicNode as CluePopUpSO;
            cluePopUpUI.CluePopUp(cluePopUp.clue);

            TryPlayNextNode();
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
