using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabControllerEvent : MonoBehaviour
{
    XRDirectInteractor directInteractor;

    // Start is called before the first frame update
    void Start()
    {
        directInteractor = GetComponent<XRDirectInteractor>();

        directInteractor.selectEntered.AddListener(GrabObjectEnter);
        directInteractor.selectExited.AddListener(GrabObjectExit);
    }

    private void GrabObjectEnter(SelectEnterEventArgs args)
    {
        Debug.Log($"Grab On: {args.interactableObject.transform.gameObject.name}");
        if (args.interactableObject.transform.gameObject.name == "SausageSingle")
        {
            FoxRandomMove.Instance.SetStartFoxWaitngFood();
        }
    }
    private void GrabObjectExit(SelectExitEventArgs args)
    {
        Debug.Log($"Grab Off: {args.interactableObject.transform.gameObject.name}");
        if (args.interactableObject.transform.gameObject.name == "SausageSingle")
        {
            FoxRandomMove.Instance.SetStopFoxWaitngFood();
        }
    }
}
