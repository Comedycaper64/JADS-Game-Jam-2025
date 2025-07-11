using UnityEngine;

public class PlayerOptions : MonoBehaviour
{
    private const string MUSIC_VOLUME = "MusicVolume";
    private const string SFX_VOLUME = "SFXVolume";
    private const string VOICE_VOLUME = "VoiceVolume";

    private static float MUSIC_VOLUME_DEF = 0.5f;
    private static float SFX_VOLUME_DEF = 0.5f;
    private static float VOICE_VOLUME_DEF = 0.5f;

    public static void SetMusicVolume(float newVolume)
    {
        PlayerPrefs.SetFloat(MUSIC_VOLUME, newVolume);
        PlayerPrefs.Save();
    }

    public static void SetSFXVolume(float newVolume)
    {
        PlayerPrefs.SetFloat(SFX_VOLUME, newVolume);
        PlayerPrefs.Save();
    }

    public static void SetVoiceVolume(float newVolume)
    {
        PlayerPrefs.SetFloat(VOICE_VOLUME, newVolume);
        PlayerPrefs.Save();
    }

    public static float GetMusicVolume()
    {
        if (!PlayerPrefs.HasKey(MUSIC_VOLUME))
        {
            return MUSIC_VOLUME_DEF;
        }
        else
        {
            return PlayerPrefs.GetFloat(MUSIC_VOLUME);
        }
    }

    public static float GetSFXVolume()
    {
        if (!PlayerPrefs.HasKey(SFX_VOLUME))
        {
            return SFX_VOLUME_DEF;
        }
        else
        {
            return PlayerPrefs.GetFloat(SFX_VOLUME);
        }
    }

    public static float GetVoiceVolume()
    {
        if (!PlayerPrefs.HasKey(VOICE_VOLUME))
        {
            return VOICE_VOLUME_DEF;
        }
        else
        {
            return PlayerPrefs.GetFloat(VOICE_VOLUME);
        }
    }
}
