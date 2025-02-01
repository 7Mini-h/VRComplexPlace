using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SausageScript : MonoBehaviour
{
    private Vector3 originLocation;
    private Vector3 originRocation;

    private Rigidbody rb;
    private XRGrabInteractable grabInteractable;

    private void Awake()
    {
        originLocation = transform.localPosition;
        originRocation = transform.localEulerAngles;
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!(collision.gameObject.name.Contains("FoodSafe") || collision.gameObject.name.Contains("Fence") || collision.gameObject.name.Contains("XR Origin")))
        {
            ResetSausage();
        }
        if (collision.gameObject.name.Contains("FoodSafe"))
        {
            if (!FoxRandomMove.Instance.isEating)
            {
                FoxRandomMove.Instance.SetStartEatFood();
                grabInteractable.enabled = false;
            }
        }
    }

    public void ResetSausage()
    {
        transform.localPosition = originLocation + new Vector3(0, 0.3f, 0);
        transform.localEulerAngles = originRocation;
        transform.localScale = Vector3.one;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        grabInteractable.enabled = true;
    }

    public void EatenByFox(float _scalingTime = 1f)
    {
        StartCoroutine(IeScaleDown(_scalingTime));
    }

    private IEnumerator IeScaleDown(float _scalingTime = 1f)
    {
        Vector3 startScale = transform.localScale;
        Vector3 targetScale = Vector3.zero;

        for (float i = 0; i < 1; i += _scalingTime * Time.deltaTime)
        {
            transform.localScale = Vector3.Lerp(startScale, targetScale, i);
            yield return null;
        }

        transform.localScale = targetScale;
        yield break;
    }
}
