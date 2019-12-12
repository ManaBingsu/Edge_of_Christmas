using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudienceManager : MonoBehaviour
{
    public static AudienceManager audManager;

    public enum State { Idle, Stand, Angry, Cheer }
    [SerializeField]
    private State state;
    public State sState
    {
        get { return state; }
        set
        {
            if(value == State.Cheer)
            {
                SoundManager.sm.EfcCrowdYeah();
                cheerTimer += cheerTime;
                state = value;
            }

            if(value == State.Angry)
            {
                SoundManager.sm.EfcCrowdWoo();
                cheerTimer += cheerTime;
                state = State.Cheer;
            }
            
        }
    }


    Coroutine stateCoroutine;

    //Back, left ,right, Front, left, right

    [SerializeField]
    private Audience[] audienceArray = new Audience[4];
    
    // 관중들 원래 포지션
    private Vector3[] audiencePosArray = new Vector3[4];

    [SerializeField]
    private float cheerTime;

    public float cheerTimer;

    [SerializeField]
    private float angryTime;

    private void Awake()
    {
        if (audManager == null)
            audManager = this;

        for(int i = 0; i < 4; i++)
        {
            audiencePosArray[i] = audienceArray[i].transform.position;
        }
    }

    private void Start()
    {
        state = State.Stand;
        StartCoroutine(FSM());
    }


    IEnumerator FSM()
    {
        while (true)
        {
            switch (state)
            {
                case State.Idle:
                    yield return stateCoroutine = StartCoroutine(Idle());
                    break;
                case State.Stand:
                    yield return stateCoroutine = StartCoroutine(Stand());
                    break;
                case State.Angry:
                    yield return stateCoroutine = StartCoroutine(Angry());
                    break;
                case State.Cheer:
                    yield return stateCoroutine = StartCoroutine(Cheer());
                    break;
            }
            yield return null;
        }
    }

    IEnumerator Idle()
    {
        while (state == State.Idle)
        {
            yield return null;
        }
    }
    IEnumerator Stand()
    {
        while(state == State.Stand)
        {
            for(int i = 0; i < 4; i++)
            {
                audienceArray[i].transform.position += Vector3.right * Random.Range(0.05f, 0.1f) * (int)audienceArray[i].direction * Time.deltaTime;
            }
            yield return null;
        }

    }

    IEnumerator Angry()
    {
        yield return null;
    }

    void SetCheer()
    {
        sState = State.Cheer;
    }
    IEnumerator Cheer()
    {
        Vector3[] currentPos = new Vector3[4];
        // 환호성 지르기 전 원래 자리로 돌아감
        for (int i = 0; i < 4; i++)
        {
            currentPos[i] = audienceArray[i].transform.position;
            audienceArray[i].cheerDirection = Audience.CheerDirection.Up;
        }

        float process = 0f;
        while (process < 1f)
        {
            process += Time.deltaTime * 6f;
            for (int i = 0; i < 4; i++)
            {
                audienceArray[i].transform.position = Vector3.Lerp(currentPos[i], audiencePosArray[i], process);
            }
            yield return null;
        }

        while (cheerTimer > 0)
        {
            cheerTimer -= Time.deltaTime;

            for (int i = 0; i < 4; i++)
            {
                audienceArray[i].transform.position += Vector3.up * Random.Range(4f, 5f) * (int)audienceArray[i].cheerDirection * Time.deltaTime;
            }

            yield return null;
        }

        // 환호성이 끝난 후 제자리로 돌아감
        for (int i = 0; i < 4; i++)
        {
            currentPos[i] = audienceArray[i].transform.position;
        }
        process = 0f;
        while(process < 1f)
        {
            process += Time.deltaTime * 6f;
            for (int i = 0; i < 4; i++)
            {
                audienceArray[i].transform.position = Vector3.Lerp(currentPos[i], audiencePosArray[i], process);
            }
            yield return null;
        }

        sState = State.Stand;
    }

}
