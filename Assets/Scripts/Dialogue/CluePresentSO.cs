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
    private ConversationSO wrongClueConversation;

    [SerializeField]
    private ConversationSO correctClueConversation;

    public string GetClueQuestion()
    {
        return clueQuestionText;
    }

    public string GetRequiredKey()
    {
        return requiredClueKey;
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
