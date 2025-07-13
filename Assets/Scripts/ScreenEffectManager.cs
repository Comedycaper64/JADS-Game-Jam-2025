using System;
using System.Collections;
using UnityEngine;

public class ScreenEffectManager : MonoBehaviour
{
    [SerializeField]
    private bool initialDarkState = true;

    [SerializeField]
    private bool initialCourtState = true;

    [SerializeField]
    private Animator effectAnimator;

    [SerializeField]
    private CanvasGroupFader darknessFader;

    [SerializeField]
    private CanvasGroupFader courtRoomFader;

    private Action OnEffectComplete;

    private void Start()
    {
        if (initialDarkState)
        {
            darknessFader.SetCanvasGroupAlpha(1f);
        }
        else
        {
            darknessFader.SetCanvasGroupAlpha(0f);
        }

        if (initialCourtState)
        {
            courtRoomFader.SetCanvasGroupAlpha(1f);
            courtRoomFader.ToggleBlockRaycasts(true);
        }
        else
        {
            courtRoomFader.SetCanvasGroupAlpha(0f);
            courtRoomFader.ToggleBlockRaycasts(false);
        }
    }

    private IEnumerator DelayedEffectComplete(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        if (OnEffectComplete != null)
        {
            OnEffectComplete();
            OnEffectComplete = null;
        }
    }

    public void PlayEffect(ScreenEffectSO screenEffect)
    {
        OnEffectComplete = screenEffect.onEffectComplete;

        switch (screenEffect.screenEffect)
        {
            case ScreenEffect.screenShake:
                //Pulse Camera? Maybe just character sprite
                OnEffectComplete();
                break;
            case ScreenEffect.whiteFlash:
                effectAnimator.SetTrigger("flash");
                OnEffectComplete();
                break;
            case ScreenEffect.fadeFromBlack:
                darknessFader.ToggleFade(false);
                StartCoroutine(DelayedEffectComplete(1f));
                break;
            case ScreenEffect.fadeToBlack:
                darknessFader.ToggleFade(true);
                StartCoroutine(DelayedEffectComplete(1f));
                break;
            case ScreenEffect.enableCourt:
                courtRoomFader.SetCanvasGroupAlpha(1f);
                courtRoomFader.ToggleBlockRaycasts(true);
                OnEffectComplete();
                break;
            case ScreenEffect.disableCourt:
                courtRoomFader.SetCanvasGroupAlpha(0f);
                courtRoomFader.ToggleBlockRaycasts(false);
                OnEffectComplete();
                break;
            case ScreenEffect.cluePresent:
                //Toggle present effect
                effectAnimator.SetTrigger("present");
                StartCoroutine(DelayedEffectComplete(2f));
                break;
            default:
                break;
        }
    }
}
