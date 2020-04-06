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

    [SerializeField]
    float MissThreshold = 0.8f;

    [SerializeField]
    float ComboTimeout = 2.2f;

    public UnityEvent OnHitEvent = new UnityEvent();
    public UnityEvent OnSwingEvent = new UnityEvent();

    private int Score;
    private int Combo = 0;
    private float MissTimer = 9999.0f;

    void Start()
    {
        OnHitEvent.AddListener(OnHit);
        OnSwingEvent.AddListener(OnSwing);
    }

    void OnHit()
    {
        MissTimer = ComboTimeout;
        Score += 5 + Combo++;
        Debug.Log(Combo);
        UIScore.text = FormatScore(Score);
    }

    private void Update()
    {
        MissTimer -= Time.deltaTime;

        if (MissTimer <= 0.0f)
        {
            Combo = 0;
            MissTimer = ComboTimeout;
        }
    }

    void OnSwing()
    {
        Debug.Log("Hit!");
        MissTimer = MissThreshold;
    }

    string FormatScore(int score)
    {
        string s = "" + score;

        if (score < 10)  s = "0" + s;
        if (score < 100) s = "0" + s;

        return s;
    }
}
