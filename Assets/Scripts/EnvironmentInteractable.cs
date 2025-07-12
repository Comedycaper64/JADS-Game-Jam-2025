using UnityEngine;
using UnityEngine.EventSystems;

public class EnvironmentInteractable : MonoBehaviour, IPointerClickHandler
{
    private bool firstTimeInteract = true;
    private EnvironmentManager environmentManager;

    [SerializeField]
    private ConversationSO interactConversation;

    private void ResolveEnvironment()
    {
        // Test to see if can end investigation
        environmentManager.TryEndInvestigation();
    }

    public void SetManager(EnvironmentManager environmentManager)
    {
        this.environmentManager = environmentManager;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (environmentManager == null)
        {
            return;
        }

        //If environment manager is busy
        if (environmentManager.GetIsBusy())
        {
            return;
        }

        if (firstTimeInteract)
        {
            environmentManager.NecessaryInteractionAdvance();

            firstTimeInteract = false;
        }

        environmentManager.SetIsBusy(true);

        CinematicManager.Instance.PlayCinematic(interactConversation, ResolveEnvironment);
    }
}
