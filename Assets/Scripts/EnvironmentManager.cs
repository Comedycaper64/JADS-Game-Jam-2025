using System;
using System.Collections;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    private int necessaryInteractCounter = 0;
    private float sensationDialogueCooldown = 5f;
    private bool isBusy = true;
    private bool sensationCooldown = false;

    [SerializeField]
    private EnvironmentInteractable[] necessaryInteractables;

    public Action OnInvestigationFinished;

    public static Action OnSensationDialogue;

    private void Start()
    {
        foreach (EnvironmentInteractable interactable in necessaryInteractables)
        {
            interactable.SetManager(this);
        }

        isBusy = true;
    }

    private void Update()
    {
        if (isBusy)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && !sensationCooldown)
        {
            StartCoroutine(DelayedClickTest());
        }
    }

    private IEnumerator DelayedUnbusy()
    {
        yield return new WaitForSeconds(1f);
        SetIsBusy(false);
    }

    private IEnumerator DelayedClickTest()
    {
        yield return new WaitForSeconds(0.1f);

        if (!isBusy)
        {
            OnSensationDialogue?.Invoke();

            StartCoroutine(SensationCD());
        }
    }

    private IEnumerator SensationCD()
    {
        sensationCooldown = true;
        yield return new WaitForSeconds(sensationDialogueCooldown);

        sensationCooldown = false;
    }

    public void BeginInvestigation()
    {
        isBusy = false;
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
            OnInvestigationFinished?.Invoke();
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
