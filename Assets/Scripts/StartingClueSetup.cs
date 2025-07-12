using UnityEngine;

public class StartingClueSetup : MonoBehaviour
{
    [SerializeField]
    private ClueSO startingClue;

    public ClueSO GetFirstClue()
    {
        return startingClue;
    }
}
