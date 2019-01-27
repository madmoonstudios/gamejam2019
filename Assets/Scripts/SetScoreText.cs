using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetScoreText : MonoBehaviour, IScoreObserver
{
    [SerializeField]
    private TextMeshProUGUI _scoreText;

    public void SetScore(int score)
    {
        _scoreText.text = string.Format("Buyers spooked: {0}", score);
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager._instance.AttachScoreObserver(this);
    }
    
}
