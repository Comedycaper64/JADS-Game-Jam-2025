using System.Collections;
using UnityEngine;

public class InvestigationStartUI : MonoBehaviour
{
    [SerializeField]
    private float showTime = 4f;

    [SerializeField]
    private CanvasGroupFader fader;

    private void Start()
    {
        EnvironmentManager.OnInvestigationStart += ShowStartUI;

        fader.SetCanvasGroupAlpha(0f);
        fader.ToggleBlockRaycasts(false);
    }

    private void OnDisable()
    {
        EnvironmentManager.OnInvestigationStart -= ShowStartUI;
    }

    private void ShowStartUI()
    {
        fader.ToggleFade(true);
        fader.ToggleBlockRaycasts(true);

        StartCoroutine(DelayedFadeOut());
    }

    private IEnumerator DelayedFadeOut()
    {
        yield return new WaitForSeconds(showTime);
        fader.ToggleFade(false);
        fader.ToggleBlockRaycasts(false);
    }
}
