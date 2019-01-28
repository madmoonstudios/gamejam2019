using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncrementVolume : MonoBehaviour
{
    public float secondsWaitPerIcremnet = 1;
    public float volumeIncrement = 0.01f;
    public float volumeLimit = 0.7f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(IncrementalVolumeUp());
    }

    private IEnumerator IncrementalVolumeUp()
    {
        var audioComponent = this.gameObject.GetComponent<AudioSource>();

        while (audioComponent.volume < volumeLimit)
        {
            yield return new WaitForSeconds(this.secondsWaitPerIcremnet);

            Debug.Log(audioComponent.volume);
            if (audioComponent.volume < volumeLimit)
            {
                audioComponent.volume += volumeIncrement;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
