using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRotator : MonoBehaviour
{
    public float speed;
    public bool isCCW = false;

    public RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        float dir;
        if (isCCW) { dir = -1f; }
        else { dir = 1f; }

        //rectTransform.Rotate(new Vector3(0, dir * speed * Time.deltaTime, 0));
        rectTransform.localEulerAngles += new Vector3(0, dir * speed * Time.deltaTime, 0);
    }
}
