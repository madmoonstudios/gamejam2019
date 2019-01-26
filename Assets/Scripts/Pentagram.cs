using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pentagram : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro _countdownText;

    [SerializeField]
    private int _maxTimeSeconds;
    
    private float _fearRadius;

    [SerializeField]
    private MeshRenderer _collisionMesh;

    // Start is called before the first frame update
    void Start()
    {
        _fearRadius = _collisionMesh.transform.localScale.x;
        StartCoroutine(ScareThenDissapear());
    }

    private IEnumerator ScareThenDissapear()
    {
        _countdownText.text = "-";
        yield return new WaitForSeconds(2.0f);

        for (int timeLeft = _maxTimeSeconds; timeLeft > 0; timeLeft--)
        {
            _collisionMesh.enabled = false;
            _countdownText.text = timeLeft.ToString();

            yield return new WaitForSeconds(0.9f);
            RaycastHit2D [] hits = Physics2D.CircleCastAll(this.transform.position, _fearRadius, Vector2.zero);

            foreach (RaycastHit2D hit in hits)
            {
                IFearable fearable = hit.transform.GetComponent<IFearable>();
                if (fearable != null)
                {
                    fearable.Scare();
                }
            }

            _collisionMesh.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }

        Destroy(this.gameObject);
    }
}
