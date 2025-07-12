using System.Collections;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    private int necessaryInteractCounter = 0;
    private bool isBusy = false;

    [SerializeField]
    private EnvironmentInteractable[] necessaryInteractables;

    [SerializeField]
    private ConversationSO endOfInvestigationConvo;

    private void Start()
    {
        foreach (EnvironmentInteractable interactable in necessaryInteractables)
        {
            interactable.SetManager(this);
        }
    }

    private IEnumerator DelayedUnbusy()
    {
        yield return new WaitForSeconds(1f);
        SetIsBusy(false);
    }

    public void NecessaryInteractionAdvance()
    {
        necessaryInteractCounter++;
    }

    public void TryEndInvestigation()
    {
        if (necessaryInteractCounter < necessaryInteractables.Length)
        {
            StartCoroutine(DelayedUnbusy());
        }
        else
        {
            CinematicManager.Instance.PlayCinematic(endOfInvestigationConvo, null);
        }
    }

    public void SetIsBusy(bool isBusy)
    {
        this.isBusy = isBusy;
    }

    public bool GetIsBusy()
    {
        return isBusy;
    }
}
