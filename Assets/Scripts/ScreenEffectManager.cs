using System;
using UnityEngine;

public class ScreenEffectManager : MonoBehaviour
{
    private Action OnEffectComplete;

    public void PlayEffect(ScreenEffectSO screenEffect)
    {
        OnEffectComplete = screenEffect.onEffectComplete;

        switch (screenEffect.screenEffect)
        {
            case ScreenEffect.screenShake:
                //Pulse Camera? Maybe just character sprite
                break;
            case ScreenEffect.whiteFlash:
                //Play canvas animation of flash
                break;
            case ScreenEffect.fadeFromBlack:
                //Toggle darkness fade
                break;
            case ScreenEffect.fadeToBlack:
                //Toggle darkness fade off
                break;
            case ScreenEffect.cluePresent:
                //Toggle present effect
                break;
            default:
                break;
        }
    }
}
