using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextDumper : MonoBehaviour
{
    [SerializeField]
    private GameObject _textObject;

    [SerializeField]
    private List<string> text;
    private bool _clicked = true;
    private int index = 0;

    private GameObject currentText;

    void Start()
    {
        ShowNextText();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShowNextText();
        }
    }

    private void ShowNextText()
    {
        if (index >= text.Count)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(2);
        }
        else
        {
            StartCoroutine(LerpCurrentTextAlphaOut());

            currentText = GameObject.Instantiate(
                _textObject,
                this.transform.position + new Vector3(UnityEngine.Random.Range(-7f, 7f),
                    UnityEngine.Random.Range(-3f, -18f), UnityEngine.Random.Range(-5f, 5f)),
                Quaternion.Euler(90, 0, 0),
                null);

            float scaleFactor = UnityEngine.Random.Range(.6f, 1.2f);
            currentText.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1.0f );
            currentText.GetComponent<TextMeshPro>().text = text[index];
            currentText.GetComponent<TextMeshPro>().alpha = 0.0f;
            
            StartCoroutine(LerpCurrentTextAlphaIn());

            index++;
        }
    }
    
    private IEnumerator LerpCurrentTextAlphaIn()
    {
        if (currentText != null)
        {
            TextMeshPro textToLerp = currentText.GetComponent<TextMeshPro>();
            while (textToLerp.alpha <= 1)
            {
                textToLerp.alpha -= UnityEngine.Random.Range(-.075f, .05f);

                yield return 0; //new WaitForSeconds(.1f);
            }
        }
    }

    private IEnumerator LerpCurrentTextAlphaOut()
    {
        if (currentText != null)
        {
            TextMeshPro textToLerp = currentText.GetComponent<TextMeshPro>();
            while (textToLerp.alpha > 0)
            {
                textToLerp.alpha += UnityEngine.Random.Range(-.075f, .05f);

                yield return 0; //new WaitForSeconds(.1f);
            }

            Destroy(textToLerp.gameObject);
        }
    }

    /*private IEnumerator DumpTextRoutine()
    {
        for (int i = 0; i < text.Count; i++)
        {
            float rTime = UnityEngine.Random.Range(1.0f, 2.0f);
            float tPassed = 0.0f;
            
            GameObject.Instantiate(
                _textObject,
                this.transform.position + new Vector3(UnityEngine.Random.Range(-5f, 5f), UnityEngine.Random.Range(-5f, -15f), UnityEngine.Random.Range(-5f, 5f)),
                Quaternion.Euler(90, 0, 0),
                null).GetComponent<SpookyWord>().SetText(text[i]);

            while (!_clicked && tPassed < rTime)
            {
                tPassed += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            //_clicked = false;
        }

        //UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }*/
}
