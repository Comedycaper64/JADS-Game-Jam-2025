using UnityEngine;

public class ButtonSFXPlayer : MonoBehaviour
{
    private float clickSfxVolume = 1f;

    [SerializeField]
    private AudioClip clickSFX;

    public void ClickSound()
    {
        AudioManager.PlaySFX(clickSFX, clickSfxVolume, 0, Camera.main.transform.position);
    }
}
