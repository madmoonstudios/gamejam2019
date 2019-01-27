using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorMovement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rigidbody;

    [SerializeField]
    private float _movementForce;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RandomMovementRoutine());
    }

    private IEnumerator RandomMovementRoutine()
    {
        while (true)
        {
            _rigidbody.AddForce(new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized * _movementForce);
            yield return new WaitForSeconds(1.0f);
        }
    }
}
