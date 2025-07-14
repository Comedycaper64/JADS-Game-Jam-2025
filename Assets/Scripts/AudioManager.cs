using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private enum PitchEnum
    {
        normal,
        twentyFive,
        fifty,
        seventyFive,
        onetwentyfive,
        onefifty,
        oneSeventyFive,
        twohundred
    }

    private const float MIN_PITCH_VARIATION = 0.8f;
    private const float MAX_PITCH_VARIATION = 1.2f;
    private const float FADE_SPEED = 0.25f;

    private static int sfxPoolCounter = 0;
    private float fadeCounter = 0f;

    private bool fadeIn = false;
    private bool fadeOut = false;

    [SerializeField]
    private AudioSource musicAudioSource;

    [SerializeField]
    private AudioSource[] localSfxPool;
    private static AudioSource[] sfxPool;

    private static Dictionary<PitchEnum, float> enumToPitch = new Dictionary<PitchEnum, float>();

    private void OnEnable()
    {
        PauseManager.OnMusicVolumeUpdated += UpdateMusicVolume;

        sfxPool = localSfxPool;
        sfxPoolCounter = 0;
    }

    private void OnDisable()
    {
        PauseManager.OnMusicVolumeUpdated -= UpdateMusicVolume;
    }

    private void Start()
    {
        if (enumToPitch.Count == 0)
        {
            enumToPitch.Add(PitchEnum.normal, 1f);
            enumToPitch.Add(PitchEnum.twentyFive, 0.25f);
            enumToPitch.Add(PitchEnum.fifty, 0.5f);
            enumToPitch.Add(PitchEnum.seventyFive, 0.75f);
            enumToPitch.Add(PitchEnum.onetwentyfive, 1.25f);
            enumToPitch.Add(PitchEnum.onefifty, 1.5f);
            enumToPitch.Add(PitchEnum.oneSeventyFive, 1.75f);
            enumToPitch.Add(PitchEnum.twohundred, 2f);
        }
    }

    private void Update()
    {
        if (fadeIn)
        {
            FadeIn();
        }
        else if (fadeOut)
        {
            FadeOut();
        }
    }

    private void SetMusicAudioSourceVolume(float newVolume)
    {
        fadeIn = false;
        fadeOut = false;

        musicAudioSource.volume = newVolume;
    }

    private void FadeIn()
    {
        fadeCounter += FADE_SPEED * Time.unscaledDeltaTime;

        if (fadeCounter < 1f)
        {
            float volume = Mathf.Lerp(0f, PlayerOptions.GetMusicVolume(), fadeCounter);
            musicAudioSource.volume = volume;
        }
        else
        {
            musicAudioSource.volume = PlayerOptions.GetMusicVolume();

            fadeIn = false;
        }
    }

    private void FadeOut()
    {
        fadeCounter += FADE_SPEED * 1.5f * Time.unscaledDeltaTime;

        if (fadeCounter < 1f)
        {
            float volume = Mathf.Lerp(PlayerOptions.GetMusicVolume(), 0f, fadeCounter);
            musicAudioSource.volume = volume;
        }
        else
        {
            fadeOut = false;
        }
    }

    private void FadeOutMusic()
    {
        fadeOut = true;
        fadeIn = false;
        fadeCounter = 0f;
    }

    private void FadeInMusic()
    {
        musicAudioSource.Play();
        SetMusicAudioSourceVolume(0f);
        fadeIn = true;
        fadeOut = false;
        fadeCounter = 0f;
    }

    public void StartMusic()
    {
        FadeInMusic();
    }

    public void ChangeMusicTrack(AudioClip newMusic)
    {
        musicAudioSource.clip = newMusic;
        FadeInMusic();
    }

    public void StopMusic()
    {
        //FadeOutMusic();
        musicAudioSource.Stop();
    }

    private static AudioSource PlaySFXClip(
        AudioClip clip,
        Vector3 position,
        float volume,
        float pitch
    )
    {
        AudioSource source = sfxPool[sfxPoolCounter];
        source.transform.position = position;
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;

        source.Play();

        sfxPoolCounter++;

        if (sfxPoolCounter >= sfxPool.Length)
        {
            sfxPoolCounter = 0;
        }

        return source;
    }

    public static AudioSource PlaySFX(
        AudioClip clip,
        float volume,
        int pitchEnum,
        Vector3 originPosition,
        bool varyPitch = true
    )
    {
        if (!Application.isPlaying)
        {
            return null;
        }

        float pitchVariance = 1f;

        if (varyPitch)
        {
            pitchVariance = Random.Range(MIN_PITCH_VARIATION, MAX_PITCH_VARIATION);
        }

        return PlaySFXClip(
            clip,
            originPosition,
            volume * PlayerOptions.GetSFXVolume(),
            enumToPitch[(PitchEnum)pitchEnum] * pitchVariance
        );
    }

    private void UpdateMusicVolume(object sender, float newVolume)
    {
        SetMusicAudioSourceVolume(PlayerOptions.GetMusicVolume());
    }
}
