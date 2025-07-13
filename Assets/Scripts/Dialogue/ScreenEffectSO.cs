using System;
using UnityEngine;

public enum ScreenEffect
{
    screenShake,
    whiteFlash,
    fadeToBlack,
    fadeFromBlack,
    enableCourt,
    disableCourt,
    cluePresent
}

[CreateAssetMenu(
    fileName = "ScreenEffect",
    menuName = "Conversation Node/ScreenEffectSO",
    order = 0
)]
public class ScreenEffectSO : ConversationNode
{
    public ScreenEffect screenEffect;

    public Action onEffectComplete;
}
