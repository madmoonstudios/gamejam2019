using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonologueGenerator : MonoBehaviour
{
    [System.Serializable]
    private class SpookyPhrase
    {
        [SerializeField]
        public string phrase;

        [SerializeField]
        public float spookLevel;
    }

    [SerializeField]
    private List<SpookyPhrase> _spookyPhrasesInput;

    private SortedList<float, string> _spookyPhrases;

    [SerializeField]
    private BuyerController _controller;
    [SerializeField]
    private GameObject _spookyWord;

    // Start is called before the first frame update
    void Start()
    {
        _spookyPhrases = new SortedList<float, string>();

        foreach (SpookyPhrase phrase in _spookyPhrasesInput)
        {
            _spookyPhrases.Add(phrase.spookLevel, phrase.phrase);
        }

        StartCoroutine(SpawnPhrasesRoutine());
    }

    private IEnumerator SpawnPhrasesRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5.0f, 10.0f));
            float spookLevel = Mathf.Max(0, _controller.GetSpookLevel() - Random.Range(0,15));

            string phrase = _spookyPhrasesInput.FindLast((spookPhrase) => spookPhrase.spookLevel <= spookLevel).phrase;

            foreach (string word in phrase.Split(' '))
            {
                GameObject.Instantiate(
                    _spookyWord,
                    this.transform.position + new Vector3(Random.Range(-3.0f, 3.0f), Random.Range(-3.0f, 3.0f), 0.0f),
                    Quaternion.Euler(Random.Range(80,100), 0, 0),
                    null).GetComponent<SpookyWord>().SetText(word);
                yield return new WaitForSeconds(Random.Range(0.5f, 2.4f));
            }
        }
    }

}
