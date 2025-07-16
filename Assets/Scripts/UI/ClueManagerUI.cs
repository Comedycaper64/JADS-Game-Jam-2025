using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClueManagerUI : MonoBehaviour
{
    public static bool inventoryOpen = false;

    private bool presentMode = false;

    [SerializeField]
    private ClueUI[] clueUIs;
    private ClueSO activeClue;

    [SerializeField]
    private AudioClip clueGetSFX;

    [SerializeField]
    private AudioClip inventoryOpenSFX;

    [Header("Clue Get UI")]
    [SerializeField]
    private CanvasGroupFader clueGetFader;

    [SerializeField]
    private Image clueGetImage;

    [SerializeField]
    private TextMeshProUGUI clueGetNameText;

    [SerializeField]
    private TextMeshProUGUI clueGetDescriptionText;

    [SerializeField]
    private TextMeshProUGUI clueGetText;

    [Header("Clue Inventory UI")]
    [SerializeField]
    private CanvasGroupFader clueInventoryFader;

    [SerializeField]
    private Image activeClueImage;

    [SerializeField]
    private TextMeshProUGUI activeClueNameText;

    [SerializeField]
    private TextMeshProUGUI activeClueDescriptionText;

    [SerializeField]
    private Button inventoryCloseButton;

    [SerializeField]
    private Button cluePresentButton;

    [Header("Clue Present UI")]
    [SerializeField]
    private CanvasGroupFader cluePresentFader;

    [SerializeField]
    private TextMeshProUGUI presentHintText;

    private void Start()
    {
        clueGetFader.SetCanvasGroupAlpha(0f);
        clueGetFader.ToggleBlockRaycasts(false);
        clueInventoryFader.SetCanvasGroupAlpha(0f);
        clueInventoryFader.ToggleBlockRaycasts(false);
        cluePresentFader.SetCanvasGroupAlpha(0f);

        ClueManager.Instance.OnNewClue += ShowNewClue;
        ClueManager.Instance.OnCluePresent += TogglePresentMode;
        InputManager.Instance.OnInventoryAction += ToggleInventoryScreen;
        ClueUI.OnClueSelected += SetActiveClue;

        cluePresentButton.interactable = false;

        UpdateClueInventory();
    }

    private void OnDisable()
    {
        ClueManager.Instance.OnNewClue -= ShowNewClue;
        ClueManager.Instance.OnCluePresent -= TogglePresentMode;
        InputManager.Instance.OnInventoryAction -= ToggleInventoryScreen;
        ClueUI.OnClueSelected -= SetActiveClue;
    }

    private void ShowNewClue(object sender, ClueSO newClue)
    {
        int newClueNumber = ClueManager.Instance.GetFoundCluesAmount();
        if (newClueNumber <= 0)
        {
            newClueNumber = 1;
        }

        SetupClueUI(newClueNumber - 1, newClue);
        SetupNewClueUI(newClue);

        AudioManager.PlaySFX(clueGetSFX, 1f, 0, transform.position);

        clueGetFader.ToggleFade(true);
        clueGetFader.ToggleBlockRaycasts(true);
    }

    private void SetupClueUI(int newClueNumber, ClueSO newClue)
    {
        ClueUI newClueUI = clueUIs[newClueNumber];
        newClueUI.gameObject.SetActive(true);
        newClueUI.SetupClueUI(newClue);
    }

    private void SetupNewClueUI(ClueSO newClue)
    {
        clueGetImage.sprite = newClue.GetClueSprite();

        clueGetNameText.text = newClue.GetClueName();
        clueGetDescriptionText.text = newClue.GetClueDescription();
        clueGetText.text =
            newClue.GetClueName() + " has been added to the investigative chronicle.";
    }

    public void CloseNewClue()
    {
        clueGetFader.ToggleFade(false);
        clueGetFader.ToggleBlockRaycasts(false);

        ClueManager.Instance.FinishAddClue();
    }

    public void ToggleInventoryScreen()
    {
        if (presentMode)
        {
            return;
        }

        inventoryOpen = !inventoryOpen;

        clueInventoryFader.ToggleFade(inventoryOpen);
        clueInventoryFader.ToggleBlockRaycasts(inventoryOpen);

        AudioManager.PlaySFX(inventoryOpenSFX, 1f, 0, transform.position);
    }

    public void TogglePresentMode(object sender, bool toggle)
    {
        presentMode = toggle;

        if (toggle)
        {
            presentHintText.text = ClueManager.Instance.GetPresentClueQuestion();
        }

        clueInventoryFader.ToggleFade(toggle);
        clueInventoryFader.ToggleBlockRaycasts(toggle);
        cluePresentFader.ToggleFade(toggle);
        cluePresentButton.interactable = toggle;
        inventoryCloseButton.interactable = !toggle;

        AudioManager.PlaySFX(inventoryOpenSFX, 1f, 0, transform.position);
    }

    public void PresentClue()
    {
        ClueManager.Instance.EvaluatePresentedClue(activeClue);
    }

    private void UpdateClueInventory()
    {
        ClueSO[] clues = ClueManager.Instance.GetFoundClues();

        int i = 0;

        foreach (ClueSO clue in clues)
        {
            SetupClueUI(i, clue);
            i++;
        }

        SetActiveClue(this, clues[0]);
    }

    private void SetActiveClue(object sender, ClueSO activeClue)
    {
        this.activeClue = activeClue;

        activeClueImage.sprite = activeClue.GetClueSprite();
        activeClueNameText.text = activeClue.GetClueName();
        activeClueDescriptionText.text = activeClue.GetClueDescription();
    }
}
