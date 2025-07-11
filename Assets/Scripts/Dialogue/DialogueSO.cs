using System;
using UnityEngine;

public enum DialogueSprite
{
    neutral,
    angry,
    explaining,
    custom1,
    custom2
}

[Serializable]
public struct Dialogue
{
    public ActorSO actor;

    [TextArea]
    public string[] dialogue;
    public DialogueSprite[] dialogueSprites;
}

[Serializable]
[CreateAssetMenu(fileName = "Dialogue", menuName = "Conversation Node/DialogueSO", order = 0)]
public class DialogueSO : ConversationNode
{
    [SerializeField]
    private Dialogue[] dialogues;

    public Dialogue[] GetDialogues()
    {
        return dialogues;
    }
}
