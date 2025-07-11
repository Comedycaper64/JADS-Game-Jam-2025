using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    private bool pauseCooldown = false;
    private bool pauseActive = false;

    [SerializeField]
    private CanvasGroupFader pauseMenuFader;

    [SerializeField]
    private Slider musicSlider;

    [SerializeField]
    private Slider sfxSlider;

    [SerializeField]
    private Slider voiceSlider;

    public static EventHandler<bool> OnPauseGame;

    public static EventHandler<float> OnMusicVolumeUpdated;
    public static EventHandler<float> OnSFXVolumeUpdated;
    public static EventHandler<float> OnVoiceVolumeUpdated;

    private void Awake()
    {
        pauseMenuFader.SetCanvasGroupAlpha(0f);
        pauseMenuFader.ToggleBlockRaycasts(false);

        musicSlider.value = PlayerOptions.GetMusicVolume();
        sfxSlider.value = PlayerOptions.GetSFXVolume();
        voiceSlider.value = PlayerOptions.GetVoiceVolume();
    }

    private void OnEnable()
    {
        InputManager.OnMenuAction += TogglePause;
    }

    private void OnDisable()
    {
        InputManager.OnMenuAction -= TogglePause;
    }

    public void SetMusicVolume(float newVolume)
    {
        PlayerOptions.SetMusicVolume(newVolume);
        OnMusicVolumeUpdated?.Invoke(this, newVolume);
    }

    public void SetSFXVolume(float newVolume)
    {
        PlayerOptions.SetSFXVolume(newVolume);

        OnSFXVolumeUpdated?.Invoke(this, newVolume);
    }

    public void SetVoiceVolume(float newVolume)
    {
        PlayerOptions.SetVoiceVolume(newVolume);

        OnVoiceVolumeUpdated?.Invoke(this, newVolume);
    }

    private IEnumerator PauseCD()
    {
        pauseCooldown = true;
        yield return new WaitForSecondsRealtime(0.5f);
        pauseCooldown = false;
    }

    public void TogglePause()
    {
        if (pauseCooldown)
        {
            return;
        }

        StartCoroutine(PauseCD());

        pauseActive = !pauseActive;

        pauseMenuFader.ToggleFade(pauseActive);
        pauseMenuFader.ToggleBlockRaycasts(pauseActive);

        if (pauseActive)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }

        OnPauseGame?.Invoke(this, pauseActive);
    }
}
