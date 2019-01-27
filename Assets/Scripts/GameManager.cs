using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;

    private int _score = 0;

    private List<IScoreObserver> _observers = new List<IScoreObserver>();

    // Start is called before the first frame update
    void Awake()
    {
        _instance = this;
    }

    internal void AttachScoreObserver(IScoreObserver observer)
    {
        _observers.Add(observer);
    }

    internal void AddToScore()
    {
        _score++;

        int oldScore = PlayerPrefs.GetInt("HighScore", 0);
        int maxScore = Mathf.Max(_score, oldScore);
        PlayerPrefs.SetInt("HighScore", maxScore);

        foreach (IScoreObserver observer in _observers)
        {
            observer.SetScore(_score);
        }
    }
}
