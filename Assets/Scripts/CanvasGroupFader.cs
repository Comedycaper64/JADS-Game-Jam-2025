using UnityEngine;

public class CanvasGroupFader : MonoBehaviour
{
    [SerializeField]
    private float fadeTime = 1f;
    private bool bFade;
    private float currentAlpha = 0f;
    private float targetAlpha = 0f;
    private float tweenTimer;

    [SerializeField]
    private AnimationCurve tweenCurve;

    [SerializeField]
    private CanvasGroup canvasGroup;

    private void Update()
    {
        if (bFade)
        {
            float newAlpha = Tween(tweenTimer, 0f, fadeTime, currentAlpha, targetAlpha);
            canvasGroup.alpha = newAlpha;
            tweenTimer += Time.unscaledDeltaTime;

            if (newAlpha == targetAlpha)
            {
                bFade = false;
            }
        }
    }

    private float Remap(float x, float A, float B, float C, float D)
    {
        float remappedValue = C + (x - A) / (B - A) * (D - C);
        return remappedValue;
    }

    private float Tween(
        float currentTime,
        float initialTime,
        float endTime,
        float startValue,
        float endValue
    )
    {
        currentTime = Remap(currentTime, initialTime, endTime, 0f, 1f);
        currentTime = tweenCurve.Evaluate(currentTime);

        return startValue + currentTime * (endValue - startValue);
    }

    private void FadeIn(float targetAlpha)
    {
        this.targetAlpha = targetAlpha;
    }

    private void FadeOut()
    {
        targetAlpha = 0f;
    }

    public void SetCanvasGroupAlpha(float alpha)
    {
        canvasGroup.alpha = alpha;
        bFade = false;
    }

    public float GetCanvasGroupAlpha()
    {
        return canvasGroup.alpha;
    }

    public void ButtonToggleFade(bool toggle)
    {
        ToggleFade(toggle);
    }

    public void ToggleBlockRaycasts(bool toggle)
    {
        canvasGroup.blocksRaycasts = toggle;
    }

    public void ToggleFade(bool toggle, float targetAlpha = 1.0f)
    {
        if (toggle)
        {
            FadeIn(targetAlpha);
        }
        else
        {
            FadeOut();
        }

        bFade = true;
        tweenTimer = 0f;
        currentAlpha = canvasGroup.alpha;
    }
}
