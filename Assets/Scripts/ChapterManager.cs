using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChapterManager : MonoBehaviour
{
    [SerializeField]
    private bool playOnstart = true;

    [SerializeField]
    private EnvironmentManager environmentManager;

    [SerializeField]
    private ConversationSO introConversation;

    [SerializeField]
    private ConversationSO endOfInvestigationConvo;

    public Action OnFinishGame;

    private void Start()
    {
        if (playOnstart)
        {
            PlayIntroConversation();
        }

        EnvironmentManager.OnInvestigationFinished += PlayOutroConversation;
    }

    private void OnDisable()
    {
        EnvironmentManager.OnInvestigationFinished -= PlayOutroConversation;
    }

    private void StartInvestigation()
    {
        environmentManager.BeginInvestigation();
    }

    private void LoadNextScene()
    {
        //if last scene, go back to initial one

        int activeBuildIndex = SceneManager.GetActiveScene().buildIndex;

        if (activeBuildIndex >= 3)
        {
            OnFinishGame?.Invoke();
        }
        else
        {
            SceneManager.LoadScene(activeBuildIndex + 1);
        }
    }

    public void PlayIntroConversation()
    {
        CinematicManager.Instance.PlayCinematic(introConversation, StartInvestigation);
    }

    private void PlayOutroConversation()
    {
        CinematicManager.Instance.PlayCinematic(endOfInvestigationConvo, LoadNextScene);
    }
}
