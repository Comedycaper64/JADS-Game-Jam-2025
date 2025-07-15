using System;
using System.Collections.Generic;
using UnityEngine;

public class ClueManager : MonoBehaviour
{
    public static ClueManager Instance { get; private set; }

    [SerializeField]
    private StartingClueSetup setup;

    private List<ClueSO> obtainedClues = new List<ClueSO>();

    private CluePresentSO currentCluePresent;

    private Action OnClueResolve;
    private Action OnCinematicFinishedSave;

    public EventHandler<ClueSO> OnNewClue;
    public EventHandler<bool> OnCluePresent;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        transform.SetParent(null);

        DontDestroyOnLoad(this);

        if (setup)
        {
            ClueSO startingClue = setup.GetFirstClue();

            obtainedClues.Add(startingClue);
        }
    }

    public void AddClue(ClueSO clue, Action onClueResolve)
    {
        //When clue dialogue is confirmed, do onClueResolve;

        if (!obtainedClues.Contains(clue))
        {
            obtainedClues.Add(clue);
            OnClueResolve = onClueResolve;
            OnNewClue?.Invoke(this, clue);
        }
        else
        {
            onClueResolve();
        }
    }

    public void FinishAddClue()
    {
        if (OnClueResolve != null)
        {
            OnClueResolve();
            OnClueResolve = null;
        }
    }

    public void StartCluePresent(CluePresentSO cluePresent, Action OnCinematicFinishedSave)
    {
        //Takes over cinematic duties for a while, handling playing incorrect clue dialogue and starting new cinematic if correct
        currentCluePresent = cluePresent;
        this.OnCinematicFinishedSave = OnCinematicFinishedSave;
        OnCluePresent?.Invoke(this, true);
    }

    private void RestartCluePresent()
    {
        OnCluePresent?.Invoke(this, true);
    }

    public void EvaluatePresentedClue(ClueSO presentedClue)
    {
        OnCluePresent?.Invoke(this, false);

        if (currentCluePresent.EvaluateKey(presentedClue.GetClueKey()))
        {
            FinishCluePresent();
        }
        else
        {
            CinematicManager.Instance.PlayCinematic(
                currentCluePresent.GetWrongClueConversation(),
                RestartCluePresent
            );
        }
    }

    private void FinishCluePresent()
    {
        CinematicManager.Instance.PlayCinematic(
            currentCluePresent.GetCorrectClueConversation(),
            OnCinematicFinishedSave
        );
        OnCinematicFinishedSave = null;
        currentCluePresent = null;
    }

    public int GetFoundCluesAmount()
    {
        return obtainedClues.Count;
    }

    public string GetPresentClueQuestion()
    {
        return currentCluePresent.GetClueQuestion();
    }

    public ClueSO[] GetFoundClues()
    {
        return obtainedClues.ToArray();
    }
}
