using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionManager : MonoBehaviour
{
    public static PlayerInteractionManager _instance;

    [SerializeField]
    private GameObject _pentagram;

    // Start is called before the first frame update
    void Awake()
    {
        _instance = this;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

            Vector3 adjustedWorldPos = new Vector3(worldPos.x, worldPos.y, 0);

            Instantiate(_pentagram, adjustedWorldPos, Quaternion.identity);
        }
    }
}
