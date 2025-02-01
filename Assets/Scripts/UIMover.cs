using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIMover : MonoBehaviour
{
    public float speed;
    public float multiple;
    public AnimationCurve graph;

    public TextMeshProUGUI boardText;
    RectTransform rectTransform;

    float Timer;
    Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.anchoredPosition3D;

        boardText.text = "<Fox Feed>\r\nGrab the <color=#A14A35>Sausage</color> and <color=red>Throw</color> it into the fence.";
    }

    void Update()
    {
        Timer += Time.deltaTime;
        rectTransform.anchoredPosition3D = startPosition + new Vector3(0, multiple * graph.Evaluate(speed * Timer), 0);
    }
}
