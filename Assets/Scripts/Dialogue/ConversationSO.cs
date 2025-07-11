using UnityEngine;

[CreateAssetMenu(
    fileName = "ConversationSO",
    menuName = "Conversation Node/ConversationSO",
    order = 0
)]
public class ConversationSO : ScriptableObject
{
    [SerializeField]
    private ConversationNode[] conversationNodes;

    public ConversationNode[] GetConversationNodes()
    {
        return conversationNodes;
    }
}
