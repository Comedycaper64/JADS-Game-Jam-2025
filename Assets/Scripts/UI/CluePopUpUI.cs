using UnityEngine;
using UnityEngine.UI;

public class CluePopUpUI : MonoBehaviour
{
    [SerializeField]
    private Image clueImage;

    [SerializeField]
    private CanvasGroupFader fader;

    [SerializeField]
    private AudioClip popUpSFX;

    private void Start()
    {
        fader.SetCanvasGroupAlpha(0f);
    }

    public void CluePopUp(ClueSO clue)
    {
        if (clue == null)
        {
            fader.ToggleFade(false);
        }
        else
        {
            clueImage.sprite = clue.GetClueSprite();
            clueImage.SetNativeSize();
            fader.ToggleFade(true);
        }

        AudioManager.PlaySFX(popUpSFX, 1f, 0, Camera.main.transform.position);
    }
}
