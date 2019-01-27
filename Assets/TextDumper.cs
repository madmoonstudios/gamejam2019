using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDumper : MonoBehaviour
{
    [SerializeField]
    private GameObject _textObject;

    [SerializeField]
    private List<string> text;
    private bool _clicked;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DumpTextRoutine());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _clicked = true;
        }
        else
        {
            _clicked = false;
        }

    }

    private IEnumerator DumpTextRoutine()
    {
        for (int i = 0; i < text.Count; i++)
        {
            float rTime = UnityEngine.Random.Range(1.0f, 2.0f);
            float tPassed = 0.0f;

            while (!_clicked && tPassed < rTime)
            {
                tPassed += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            _clicked = false;
            GameObject.Instantiate(
                _textObject,
                this.transform.position + new Vector3(UnityEngine.Random.Range(-5f, 5f), UnityEngine.Random.Range(-5f, -15f), UnityEngine.Random.Range(-5f, 5f)),
                Quaternion.Euler(90, 0, 0),
                null).GetComponent<SpookyWord>().SetText(text[i]);
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }
}
