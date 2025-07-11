using UnityEngine;

[CreateAssetMenu(fileName = "PlaySFX", menuName = "Conversation Node/PlaySFXSO", order = 0)]
public class PlaySFXSO : ConversationNode
{
    public AudioClip soundEffect;

    [Range(0, 1)]
    public float sfxVolume;

    [Range(0, 7)]
    public int sfxPitch;
}
