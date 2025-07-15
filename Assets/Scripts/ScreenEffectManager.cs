using System;
using System.Collections;
using UnityEngine;

public class ScreenEffectManager : MonoBehaviour
{
    //private Coroutine delayCoroutine;

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

    [SerializeField]
    private AudioClip screenFlashSFX;

    [SerializeField]
    private AudioClip cluePresentSFX;

    private Action OnEffectComplete;

    private void Awake()
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
        //Debug.Log("Delay Started");
        yield return new WaitForSeconds(delayTime);

        if (OnEffectComplete != null)
        {
            OnEffectComplete();
            //Debug.Log("Delay Finished");
            //OnEffectComplete = null;
        }
    }

    public void PlayEffect(ScreenEffectSO screenEffect)
    {
        OnEffectComplete = screenEffect.onEffectComplete;

        // if (delayCoroutine != null)
        // {
        //     StopCoroutine(delayCoroutine);
        // }

        switch (screenEffect.screenEffect)
        {
            case ScreenEffect.screenShake:
                //Pulse Camera? Maybe just character sprite
                OnEffectComplete();
                break;
            case ScreenEffect.whiteFlash:
                effectAnimator.SetTrigger("flash");
                AudioManager.PlaySFX(screenFlashSFX, 0.5f, 0, Camera.main.transform.position);
                OnEffectComplete();
                break;
            case ScreenEffect.fadeFromBlack:
                darknessFader.ToggleFade(false);
                StartCoroutine(DelayedEffectComplete(2f));
                break;
            case ScreenEffect.fadeToBlack:
                darknessFader.ToggleFade(true);
                StartCoroutine(DelayedEffectComplete(2f));
                break;
            case ScreenEffect.enableCourt:
                courtRoomFader.SetCanvasGroupAlpha(1f);
                courtRoomFader.ToggleBlockRaycasts(true);
                StartCoroutine(DelayedEffectComplete(0.1f));
                break;
            case ScreenEffect.disableCourt:
                courtRoomFader.SetCanvasGroupAlpha(0f);
                courtRoomFader.ToggleBlockRaycasts(false);
                StartCoroutine(DelayedEffectComplete(0.1f));
                break;
            case ScreenEffect.cluePresent:
                //Toggle present effect
                effectAnimator.SetTrigger("present");
                AudioManager.PlaySFX(cluePresentSFX, 0.5f, 0, Camera.main.transform.position);
                StartCoroutine(DelayedEffectComplete(2f));
                break;
            default:
                break;
        }
    }
}
