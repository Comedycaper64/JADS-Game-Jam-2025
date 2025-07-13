using UnityEngine;

public class SensationUI : MonoBehaviour
{
    private int bubbleIndex;

    [SerializeField]
    private SensationDialogueBubbleUI[] dialogueBubbles;

    private void Start()
    {
        EnvironmentManager.OnSensationDialogue += SpawnDialogueBubble;
    }

    private void OnDisable()
    {
        EnvironmentManager.OnSensationDialogue -= SpawnDialogueBubble;
    }

    private void SpawnDialogueBubble()
    {
        dialogueBubbles[bubbleIndex].ActivateBubble();

        bubbleIndex++;

        if (bubbleIndex >= dialogueBubbles.Length)
        {
            bubbleIndex = 0;
        }
    }
}
