using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Game : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI UIScore = null;

    [SerializeField]
    TextMeshProUGUI UIHighscore = null;

    [SerializeField]
    TextMeshProUGUI UITimer = null;

    [SerializeField]
    Button UIBtnStart = null;

    [SerializeField]
    AudioSource BlipSound = null;

    [SerializeField]
    AudioSource BlipSound2 = null;

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

    enum EState { Countdown, Playing, Waiting }
    EState State = EState.Waiting;

    private float Timer;
    private int Score = 0, Highscore = 0;
    private int Combo = 0;
    private float MissTimer = 9999.0f;
    private int PrevTimer = 0;

    void Start()
    {
        OnHitEvent.AddListener(OnHit);
        OnSwingEvent.AddListener(OnSwing);

        Timer = CountdownFrom;
        UITimer.text = FormatTime(CountdownFrom);
    }

    public bool IsPlaying()
    {
        return State == EState.Playing;
    }

    public void StartGame()
    {
        State = EState.Countdown;
        Timer = PrevTimer = CountdownFrom;
    }

    void OnHit()
    {
        if (IsPlaying())
        {
            MissTimer = ComboTimeout;
            Score += 5 + Combo++;
            UIScore.text = FormatScore(Score);
        }
    }

    void OnSwing()
    {
        if (IsPlaying())
        {
            MissTimer = MissThreshold;
        }
    }

    private void Update()
    {
        if (State == EState.Countdown)
        {
            Timer -= Time.deltaTime;
            var t = Mathf.CeilToInt(Timer);
            UITimer.text = FormatTime(t);

            if (PrevTimer != t)
            {
                if (t == 0)
                    BlipSound2.Play();
                else
                    BlipSound.Play();
            }

            PrevTimer = t;

            if (Timer <= -0.9f)
            {
                State = EState.Playing;
                Timer = TimeLimit;
            }
        }
        else if (State == EState.Playing)
        {
            Timer -= Time.deltaTime;
            UITimer.text = FormatTime(Mathf.CeilToInt(Timer));

            if (Timer <= -0.9f)
            {
                State = EState.Waiting;
                Timer = CountdownFrom;

                UITimer.text = FormatTime(CountdownFrom);

                if (Score > Highscore)
                {
                    Highscore = Score;
                    UIHighscore.text = FormatScore(Highscore);
                }

                Score = 0;
            }

            MissTimer -= Time.deltaTime;

            if (MissTimer <= 0.0f)
            {
                Combo = 0;
                MissTimer = ComboTimeout;
            }
        }
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
