using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndUI : MonoBehaviour
{
    [SerializeField]
    private ChapterManager chapterManager;

    [SerializeField]
    private CanvasGroupFader endScreenFader;

    [SerializeField]
    private Animator gameEndAnimator;

    [SerializeField]
    private AudioSource gameEndAudioSource;

    private void Start()
    {
        chapterManager.OnFinishGame += FinishGame;
    }

    private void OnDisable()
    {
        chapterManager.OnFinishGame -= FinishGame;
    }

    private void FinishGame()
    {
        endScreenFader.ToggleBlockRaycasts(true);

        gameEndAnimator.SetTrigger("end");
    }

    public void PlayAudioSource()
    {
        gameEndAudioSource.volume = PlayerOptions.GetSFXVolume();
        gameEndAudioSource.Play();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
