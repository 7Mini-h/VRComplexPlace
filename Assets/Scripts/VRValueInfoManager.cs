using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using UnityEngine.UI;

public class VRValueInfoManager : MonoBehaviour
{
    public InputActionAsset inputActionAsset;

    public Image[] leftControllerUI;
    public Image[] rightControllerUI;

    public GameObject[] controllerCanvases;

    public GameObject[] rayControllers;
    public GameObject[] directControllers;

    bool isCanvasChangeLeft;
    bool isCanvasChangeRight;

    bool isControllerChangeLeft;
    bool isControllerChangeRight;

    bool isRayControllerLeft;
    bool isRayControllerRight;

    private void Start()
    {
        for (int i = 0; i < leftControllerUI.Length; i++)
        {
            leftControllerUI[i].color = Color.red;
        }
        for (int i = 0; i < rightControllerUI.Length; i++)
        {
            rightControllerUI[i].color = Color.red;
        }
        for (int i = 0; i < controllerCanvases.Length; i++)
        {
            controllerCanvases[i].SetActive(false);
        }
        isCanvasChangeLeft = false;
        isCanvasChangeRight = false;

        for (int i = 0; i < directControllers.Length; i++)
        {
            directControllers[i].SetActive(false);
        }
        isRayControllerLeft = true;
        isRayControllerRight = true;
        isControllerChangeLeft = false;
        isControllerChangeRight = false;

    }
    // Update is called once per frame
    void Update()
    {
        //Vector3 HMDRotation = inputActionAsset.actionMaps[0].actions[1].ReadValue<Vector3>();

        // Controller Tracking
        if (inputActionAsset.actionMaps[1].actions[2].ReadValue<float>() == 1)
        {
            leftControllerUI[0].color = Color.green;
        }
        else
        {
            leftControllerUI[0].color = Color.red;
        }
        if (inputActionAsset.actionMaps[4].actions[2].ReadValue<float>() == 1)
        {
            rightControllerUI[0].color = Color.green;
        }
        else
        {
            rightControllerUI[0].color = Color.red;
        }

        // Controller Trigger
        if (inputActionAsset.actionMaps[2].actions[2].ReadValue<float>() == 1)
        {
            leftControllerUI[1].color = Color.green;
        }
        else
        {
            leftControllerUI[1].color = Color.red;
        }
        if (inputActionAsset.actionMaps[5].actions[2].ReadValue<float>() == 1)
        {
            rightControllerUI[1].color = Color.green;
        }
        else
        {
            rightControllerUI[1].color = Color.red;
        }

        // Controller grab
        if (inputActionAsset.actionMaps[3].actions[6].ReadValue<float>() == 1)
        {
            leftControllerUI[2].color = Color.green;
        }
        else
        {
            leftControllerUI[2].color = Color.red;
        }
        if (inputActionAsset.actionMaps[6].actions[6].ReadValue<float>() == 1)
        {
            rightControllerUI[2].color = Color.green;
        }
        else
        {
            rightControllerUI[2].color = Color.red;
        }

        // Controller PrimaryButton
        if (inputActionAsset.actionMaps[1].actions[13].ReadValue<float>() == 1)
        {
            if (!isControllerChangeLeft)
            {
                leftControllerUI[3].color = Color.green;
                isControllerChangeLeft = true;
                StartCoroutine(IeControllerChange(true));
            }

        }
        else
        {
            leftControllerUI[3].color = Color.red;
        }
        if (inputActionAsset.actionMaps[4].actions[13].ReadValue<float>() == 1)
        {
            if (!isControllerChangeRight)
            {
                rightControllerUI[3].color = Color.green;
                isControllerChangeRight = true;
                StartCoroutine(IeControllerChange(false));
            }
        }
        else
        {
            rightControllerUI[3].color = Color.red;
        }

        // Controller SecondaryButton
        if (inputActionAsset.actionMaps[1].actions[14].ReadValue<float>() == 1)
        {
            if (!isCanvasChangeLeft)
            {
                leftControllerUI[4].color = Color.green;
                isCanvasChangeLeft = true;
                StartCoroutine(IeCanvasOnOff(0));
            }
        }
        else
        {
            leftControllerUI[4].color = Color.red;
        }
        if (inputActionAsset.actionMaps[4].actions[14].ReadValue<float>() == 1)
        {
            if (!isCanvasChangeRight)
            {
                rightControllerUI[4].color = Color.green;
                isCanvasChangeRight = true;
                StartCoroutine(IeCanvasOnOff(1));
            }
        }
        else
        {
            rightControllerUI[4].color = Color.red;
        }

    }


    private IEnumerator IeControllerChange(bool isLeft)
    {
        if (isLeft)
        {
            if (isControllerChangeLeft)
            {
                if (isRayControllerLeft)
                {
                    rayControllers[0].SetActive(false);
                    directControllers[0].SetActive(true);
                    isRayControllerLeft = false;
                }
                else
                {
                    rayControllers[0].SetActive(true);
                    directControllers[0].SetActive(false);
                    isRayControllerLeft = true;
                }

                yield return new WaitForSeconds(0.5f);
                isControllerChangeLeft = false;
            }
        }
        else
        {
            if (isControllerChangeRight)
            {
                if (isRayControllerRight)
                {
                    rayControllers[1].SetActive(false);
                    directControllers[1].SetActive(true);
                    isRayControllerRight = false;
                }
                else
                {
                    rayControllers[1].SetActive(true);
                    directControllers[1].SetActive(false);
                    isRayControllerRight = true;
                }

                yield return new WaitForSeconds(0.5f);
                isControllerChangeRight = false;
            }
        }

        yield return null;
    }

    private IEnumerator IeCanvasOnOff(int canvasIndex)
    {
        if (canvasIndex == 0)
        {
            if (isCanvasChangeLeft)
            {
                controllerCanvases[canvasIndex].SetActive(!controllerCanvases[canvasIndex].activeSelf);
                yield return new WaitForSeconds(0.5f);
                isCanvasChangeLeft = false;
            }
        }
        else if (canvasIndex == 1)
        {
            if (isCanvasChangeRight)
            {
                controllerCanvases[canvasIndex].SetActive(!controllerCanvases[canvasIndex].activeSelf);
                yield return new WaitForSeconds(0.5f);
                isCanvasChangeRight = false;
            }
        }

        yield return null;
    }
}
