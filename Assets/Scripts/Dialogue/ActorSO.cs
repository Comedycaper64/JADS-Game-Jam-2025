using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Actor", menuName = "Conversation Node/ActorSO", order = 0)]
public class ActorSO : ScriptableObject
{
    [SerializeField]
    private string actorName;

    [ColorUsage(true, true)]
    [SerializeField]
    private Color actorNameColour;

    [SerializeField]
    private Sprite[] dialogueSprites;

    [SerializeField]
    private AudioClip dialogueNoises;

    public string GetActorName()
    {
        return actorName;
    }

    public Color GetActorNameColour()
    {
        return actorNameColour;
    }

    public Sprite[] GetDialogueSprites()
    {
        return dialogueSprites;
    }

    public AudioClip GetDialogueNoises()
    {
        return dialogueNoises;
    }
}
