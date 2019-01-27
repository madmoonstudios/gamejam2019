using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowNoClick : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;

    float timeSinceClick = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        text.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceClick += Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            timeSinceClick = 0.0f;
        }

        if (timeSinceClick > 10.0f)
        {
            text.text = "Please click";
        }
        else
        {
            text.text = "";
        }
    }
}
