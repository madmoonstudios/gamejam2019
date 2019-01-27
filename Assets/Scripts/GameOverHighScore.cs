﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverHighScore : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _gameOverText;
    // Start is called before the first frame update
    void Start()
    {
        _gameOverText.text = string.Format("House Haunters, High Score: {0} click to start", PlayerPrefs.GetInt("HighScore", 0));
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
    }
}