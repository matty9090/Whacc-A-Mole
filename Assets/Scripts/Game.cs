using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class HitEvent : UnityEvent<Vector3>
{

}

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
    Canvas UICanvas = null;

    [SerializeField]
    GameObject UIComboPrefab = null;

    [SerializeField]
    AudioSource BlipSound = null;

    [SerializeField]
    AudioSource BlipSound2 = null;

    [SerializeField]
    AudioSource HighscoreSound = null;

    [SerializeField]
    AudioSource GameOverSound = null;

    [SerializeField]
    int TimeLimit = 60;

    [SerializeField]
    int CountdownFrom = 3;

    [SerializeField]
    float MissThreshold = 0.8f;

    [SerializeField]
    float ComboTimeout = 2.2f;

    public UnityEvent<Vector3> OnHitEvent = new HitEvent();
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
        UIBtnStart.interactable = false;
    }

    void OnHit(Vector3 position)
    {
        if (IsPlaying())
        {
            Combo++;
            int add = Mathf.Max(0, Combo - 1);

            MissTimer = ComboTimeout;
            Score += 5 + add;
            UIScore.text = FormatScore(Score);

            if (add > 0)
            {
                GameObject combo = Instantiate(UIComboPrefab);
                combo.transform.parent = UICanvas.transform;
                combo.transform.position = Camera.main.WorldToScreenPoint(position) + Vector3.up * 40.0f;
                combo.GetComponentInChildren<TextMeshProUGUI>().text = "+" + add;
                Destroy(combo, 1.0f);
            }
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

            if (Timer <= -0.8f)
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

                UIBtnStart.interactable = true;
                UITimer.text = FormatTime(CountdownFrom);

                if (Score > Highscore)
                {
                    HighscoreSound.Play();
                    Highscore = Score;
                    UIHighscore.text = FormatScore(Highscore);
                }
                else
                {
                    GameOverSound.Play();
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
