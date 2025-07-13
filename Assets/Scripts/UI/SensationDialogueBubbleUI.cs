using System.Collections;
using UnityEngine;

public class SensationDialogueBubbleUI : MonoBehaviour
{
    [SerializeField]
    private CanvasGroupFader fader;

    public void ActivateBubble()
    {
        transform.position = Input.mousePosition;

        fader.ToggleFade(true);

        StartCoroutine(DelayedDeactive());
    }

    private IEnumerator DelayedDeactive()
    {
        yield return new WaitForSeconds(3f);
        fader.ToggleFade(false);
    }
}
