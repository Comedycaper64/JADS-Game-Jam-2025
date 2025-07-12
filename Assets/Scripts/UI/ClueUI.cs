using System;
using UnityEngine;
using UnityEngine.UI;

public class ClueUI : MonoBehaviour
{
    protected ClueSO clueSO;

    [SerializeField]
    private Image clueSprite;

    public static EventHandler<ClueSO> OnClueSelected;

    public void SetupClueUI(ClueSO clue)
    {
        clueSO = clue;
        clueSprite.sprite = clue.GetClueSprite();
    }

    public void SelectActiveClue()
    {
        OnClueSelected?.Invoke(this, clueSO);
    }
}
