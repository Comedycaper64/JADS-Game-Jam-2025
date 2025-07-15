using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Clue", menuName = "Conversation Node/ClueSO", order = 0)]
public class ClueSO : ConversationNode
{
    [SerializeField]
    private string clueName;

    [TextArea]
    [SerializeField]
    private string clueDescription;

    [SerializeField]
    private Sprite clueSprite;

    [SerializeField]
    private string clueKey;

    public string GetClueName()
    {
        return clueName;
    }

    public string GetClueDescription()
    {
        return clueDescription;
    }

    public Sprite GetClueSprite()
    {
        return clueSprite;
    }

    public string GetClueKey()
    {
        return clueKey;
    }
}
