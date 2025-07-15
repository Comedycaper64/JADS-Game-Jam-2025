using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "CluePresent", menuName = "Conversation Node/CluePresentSO", order = 0)]
public class CluePresentSO : ConversationNode
{
    [TextArea]
    [SerializeField]
    private string clueQuestionText;

    [SerializeField]
    private string requiredClueKey;

    [SerializeField]
    private string alternateClueKey;

    [SerializeField]
    private ConversationSO wrongClueConversation;

    [SerializeField]
    private ConversationSO correctClueConversation;

    public string GetClueQuestion()
    {
        return clueQuestionText;
    }

    public bool EvaluateKey(string key)
    {
        if (key == requiredClueKey)
        {
            return true;
        }
        else if (key == alternateClueKey)
        {
            return true;
        }

        return false;
    }

    public ConversationSO GetWrongClueConversation()
    {
        return wrongClueConversation;
    }

    public ConversationSO GetCorrectClueConversation()
    {
        return correctClueConversation;
    }
}
