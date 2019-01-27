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
            float spookLevel = Mathf.Max(0, _controller.GetSpookLevel() + Random.Range(-.2f, .2f));

            string phrase = _spookyPhrasesInput.FindLast((spookPhrase) => spookPhrase.spookLevel <= spookLevel).phrase;

            foreach (string word in phrase.Split(' '))
            {
                GameObject.Instantiate(
                    _spookyWord,
                    this.transform.position + new Vector3(Random.Range(-.05f, .05f), 0, Random.Range(-.05f, .05f)),
                    Quaternion.Euler(90, 0, 0),
                    null).GetComponent<SpookyWord>().SetText(word);
                yield return new WaitForSeconds(Random.Range(1.5f, 2.4f));
            }
        }
    }

}
