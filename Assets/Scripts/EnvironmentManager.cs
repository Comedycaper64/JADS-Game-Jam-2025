using System;
using System.Collections;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    private int necessaryInteractCounter = 0;
    private float sensationDialogueCooldown = 2f;
    private bool isBusy = true;
    private bool sensationCooldown = false;

    [SerializeField]
    private EnvironmentInteractable[] necessaryInteractables;

    public static EventHandler<string> OnSensationDialogue;
    public static Action OnInvestigationStart;
    public static Action OnInvestigationFinished;

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

        // if (Input.GetMouseButtonDown(0) && !sensationCooldown)
        // {
        //     StartCoroutine(DelayedClickTest());
        // }
    }

    private IEnumerator DelayedUnbusy()
    {
        yield return new WaitForSeconds(1f);
        SetIsBusy(false);
    }

    // private IEnumerator DelayedClickTest()
    // {
    //     yield return new WaitForSeconds(0.1f);

    //     if (!isBusy)
    //     {
    //         OnSensationDialogue?.Invoke();

    //         StartCoroutine(SensationCD());
    //     }
    // }

    private IEnumerator SensationCD()
    {
        sensationCooldown = true;
        yield return new WaitForSeconds(sensationDialogueCooldown);

        sensationCooldown = false;
    }

    public void BeginInvestigation()
    {
        isBusy = false;
        OnInvestigationStart?.Invoke();
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

    public void TryDisplaySensation(string sensation)
    {
        if (isBusy || sensationCooldown)
        {
            return;
        }

        OnSensationDialogue?.Invoke(this, sensation);

        StartCoroutine(SensationCD());
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
