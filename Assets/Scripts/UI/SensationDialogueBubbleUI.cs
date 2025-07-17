using System.Collections;
using TMPro;
using UnityEngine;

public class SensationDialogueBubbleUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI dialogue;

    [SerializeField]
    private CanvasGroupFader fader;

    public void ActivateBubble(string sensation)
    {
        transform.position = Input.mousePosition;
        dialogue.text = sensation;

        fader.ToggleFade(true);

        StartCoroutine(DelayedDeactive());
    }

    private IEnumerator DelayedDeactive()
    {
        yield return new WaitForSeconds(3f);
        fader.ToggleFade(false);
    }
}
