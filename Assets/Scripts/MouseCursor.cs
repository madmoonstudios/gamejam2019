using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _renderer;

    [SerializeField]
    private Sprite _vase;
    [SerializeField]
    private Sprite _light;
    [SerializeField]
    private Sprite _pentagram;

    public bool _lightHighlighted = false;
    public bool _vaseHighlighted = false;

    public static MouseCursor _instance;

    private void Awake()
    {
        _instance = this;
    }

    void Update()
    {

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //todo: make 3.66 actual floor height

        this.transform.position = new Vector3(worldPos.x, 3.66f, worldPos.z);

        if (_vaseHighlighted && !PlayerInteractionManager._instance.CannotBreakVase())
        {
            _renderer.sprite = _vase;
            return;
        }

        if (_lightHighlighted && !PlayerInteractionManager._instance.CannotSwitchLight())
        {
            _renderer.sprite = _light;
            return;
        }

        if (!PlayerInteractionManager._instance.CannotPentagram())
        {
            _renderer.sprite = _pentagram;
            return;
        }
        _renderer.sprite = null;
    }
}
