using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Game : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI UIScore = null;

    [SerializeField]
    TextMeshProUGUI UIHighscore = null;

    [SerializeField]
    TextMeshProUGUI UITimer = null;

    [SerializeField]
    GameObject UIInputBlocker = null;

    [SerializeField]
    int TimeLimit = 60;

    [SerializeField]
    int CountdownFrom = 3;

    [SerializeField]
    float MissThreshold = 0.8f;

    [SerializeField]
    float ComboTimeout = 2.2f;

    public UnityEvent OnHitEvent = new UnityEvent();
    public UnityEvent OnSwingEvent = new UnityEvent();

    enum EState { Countdown, Playing }
    EState State = EState.Countdown;

    private float Timer;
    private int Score;
    private int Combo = 0;
    private float MissTimer = 9999.0f;

    void Start()
    {
        OnHitEvent.AddListener(OnHit);
        OnSwingEvent.AddListener(OnSwing);

        Timer = CountdownFrom;
    }

    void OnHit()
    {
        MissTimer = ComboTimeout;
        Score += 5 + Combo++;
        UIScore.text = FormatScore(Score);
    }

    private void Update()
    {
        if (State == EState.Countdown)
        {
            Timer -= Time.deltaTime;
            UITimer.text = FormatTime(Mathf.CeilToInt(Timer));

            if (Timer <= -0.9f)
            {
                State = EState.Playing;
                Timer = TimeLimit;
                UIInputBlocker.SetActive(false);
            }
        }
        else
        {
            Timer -= Time.deltaTime;
            UITimer.text = FormatTime(Mathf.CeilToInt(Timer));

            MissTimer -= Time.deltaTime;

            if (MissTimer <= 0.0f)
            {
                Combo = 0;
                MissTimer = ComboTimeout;
            }
        }
    }

    void OnSwing()
    {
        MissTimer = MissThreshold;
    }

    string FormatScore(int score)
    {
        string s = "" + score;

        if (score < 10)  s = "0" + s;
        if (score < 100) s = "0" + s;

        return s;
    }

    string FormatTime(int secs)
    {
        int mins = secs / 60;
        secs -= mins * 60;
        string s = (secs < 10 ? "0" : "") + secs;

        return $"{mins}:{s}";
    }
}
