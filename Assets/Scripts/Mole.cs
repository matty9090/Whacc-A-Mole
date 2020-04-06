using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole : MonoBehaviour
{
    [SerializeField]
    float PopUpMin = 1.0f;

    [SerializeField]
    float PopUpMax = 6.0f;

    [SerializeField]
    float HideMin = 0.4f;

    [SerializeField]
    float HideMax = 1.8f;

    [SerializeField]
    float Ceiling = 2.0f;

    [SerializeField]
    float PopUpSpeed = 2.0f;

    [SerializeField]
    float HideSpeed = 1.6f;

    [SerializeField]
    Game Game = null;

    enum EState { Hiding, Visible }

    float RestY = 0.0f;
    float Timer = 0.0f;
    EState State = EState.Hiding;

    private void Start()
    {
        RestY = transform.position.y;
        Timer = Random.Range(0.0f, PopUpMax);
    }

    void Update()
    {
        if (!Game.IsPlaying())
        {
            return;
        }

        if (State == EState.Hiding)
        {
            Timer -= Time.deltaTime;

            if (Timer <= 0.0f)
            {
                StartCoroutine(PopUp());
                Timer = Random.Range(HideMin, HideMax);
                State = EState.Visible;
            }
        }
        else
        {
            Timer -= Time.deltaTime;

            if (Timer <= 0.0f)
            {
                Hide(HideSpeed);
            }
        }
    }

    void Hide(float speed)
    {
        StopAllCoroutines();
        State = EState.Hiding;
        Timer = Random.Range(PopUpMin, PopUpMax);
        StartCoroutine(PopDown(speed));
    }

    IEnumerator PopUp()
    {
        while (transform.position.y < Ceiling)
        {
            transform.position += Vector3.up * Time.deltaTime * PopUpSpeed;
            yield return null;
        }

        transform.position = new Vector3(transform.position.x, Ceiling, transform.position.z);
    }

    IEnumerator PopDown(float speed)
    {
        while (transform.position.y > RestY)
        {
            transform.position -= Vector3.up * Time.deltaTime * speed;
            yield return null;
        }

        transform.position = new Vector3(transform.position.x, RestY, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        Hide(HideSpeed * 4.0f);
        Game.OnHitEvent.Invoke(transform.position);
        GetComponent<AudioSource>().Play();
    }
}
