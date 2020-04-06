using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Game : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI UIScore = null;

    [SerializeField]
    TextMeshProUGUI UIHighscore = null;

    public UnityEvent OnHitEvent = new UnityEvent();
    public UnityEvent OnMissEvent = new UnityEvent();

    private int Score;

    void Start()
    {
        OnHitEvent.AddListener(OnHit);
        OnMissEvent.AddListener(OnMiss);
    }

    void OnHit()
    {
        Score += 5;
        UIScore.text = FormatScore(Score);
    }

    void OnMiss()
    {
        
    }

    string FormatScore(int score)
    {
        string s = "" + score;

        if (score < 10)  s = "0" + s;
        if (score < 100) s = "0" + s;

        return s;
    }
}
