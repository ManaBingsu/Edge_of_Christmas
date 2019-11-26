using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudienceManager : MonoBehaviour
{
    public enum State { Idle, Stand, Angry, Cheer, Win }
    public State state;
    Coroutine stateCoroutine;

    //Back, left ,right, Front, left, right

    [SerializeField]
    private Audience[] audienceArray = new Audience[4];

    [SerializeField]
    private float cheerTime;

    [SerializeField]
    private float angryTime;

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
                case State.Win:
                    yield return stateCoroutine = StartCoroutine(Win());
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

    IEnumerator Cheer()
    {
        yield return null;
    }
    IEnumerator Win()
    {
        yield return null;
    }
}
