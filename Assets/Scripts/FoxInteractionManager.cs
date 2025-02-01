using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxInteractionManager : MonoBehaviour
{
    public GameObject explainCanvas;

    private void Start()
    {
        explainCanvas.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "XR Origin (XR Rig)")
        {
            explainCanvas.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "XR Origin (XR Rig)")
        {
            explainCanvas.SetActive(false);
        }
    }
}
