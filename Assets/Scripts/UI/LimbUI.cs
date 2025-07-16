using UnityEngine;

public class LimbUI : MonoBehaviour
{
    private bool limbActive = false;

    [SerializeField]
    private CanvasGroupFader limbFader;

    [SerializeField]
    private Transform limbTransform;

    private Vector3 bloodOffset = new Vector3(0.2f, -0.25f, 11f);

    [SerializeField]
    private Transform limbBloodTransform;

    private void Start()
    {
        EnvironmentManager.OnInvestigationStart += EnableLimbUI;
        EnvironmentManager.OnInvestigationFinished += DisableLimbUI;

        limbBloodTransform.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        EnvironmentManager.OnInvestigationStart -= EnableLimbUI;
        EnvironmentManager.OnInvestigationFinished -= DisableLimbUI;
    }

    private void Update()
    {
        if (limbActive)
        {
            limbTransform.position = Input.mousePosition;
            limbBloodTransform.position =
                Camera.main.ScreenToWorldPoint(Input.mousePosition) + bloodOffset;
        }
    }

    private void DisableLimbUI()
    {
        limbActive = false;
        limbFader.ToggleFade(false);
        limbBloodTransform.gameObject.SetActive(false);

        Cursor.visible = true;
    }

    private void EnableLimbUI()
    {
        limbActive = true;
        limbFader.ToggleFade(true);
        limbBloodTransform.gameObject.SetActive(true);

        Cursor.visible = false;
    }
}
