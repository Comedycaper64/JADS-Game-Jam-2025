using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "CluePopUp", menuName = "Conversation Node/CluePopUpSO", order = 0)]
public class CluePopUpSO : ConversationNode
{
    public ClueSO clue;
}
