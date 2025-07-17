using UnityEngine;
using UnityEngine.EventSystems;

public class SensationInteractable : MonoBehaviour, IPointerClickHandler
{
    [TextArea]
    [SerializeField]
    private string sensationDialogue;

    [SerializeField]
    private EnvironmentManager environmentManager;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (environmentManager == null)
        {
            return;
        }

        environmentManager.TryDisplaySensation(sensationDialogue);
    }
}
