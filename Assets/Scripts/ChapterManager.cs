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

    private void Start()
    {
        if (playOnstart)
        {
            PlayIntroConversation();
        }

        environmentManager.OnInvestigationFinished += PlayOutroConversation;
    }

    private void OnDisable()
    {
        environmentManager.OnInvestigationFinished -= PlayOutroConversation;
    }

    private void StartInvestigation()
    {
        environmentManager.BeginInvestigation();
    }

    private void LoadNextScene()
    {
        //if last scene, go back to initial one

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
