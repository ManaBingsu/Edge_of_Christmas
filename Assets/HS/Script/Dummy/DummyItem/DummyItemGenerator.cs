using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyItemGenerator : MonoBehaviour
{
    public enum State { Idle, Generating }
    public State state;

    Coroutine stateCoroutine;

    // pooling 용 인덱스 키값
    [SerializeField]
    private int key;

    // 평균적으로 생성될 시간 간격
    [SerializeField]
    private float averageTime;

    [SerializeField]
    private float timeRange;

    // 아이템이 생성될 범위
    [SerializeField]
    private float xRange;

    // 아이템 평균 떨어지는 속도
    [SerializeField]
    private float averageSpeed;
    // 속도 범위
    [SerializeField]
    private float speedRange;

    // 아이템정보를 가지고 있는 아이템 리스트 참조
    public DummyItemList itemList;

    public List<DummyItemFalling> itemPoolingList;

    private void Awake()
    {
        SetItemList();
        DummySystemManager.systemManager.itemGenerator = this;
    }

    private void Start()
    {
        StartCoroutine(FSM());
    }

    void SetItemList()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            itemPoolingList.Add(transform.GetChild(i).GetComponent<DummyItemFalling>());
        }
    }

    IEnumerator FSM()
    {
        while(true)
        {
            switch(state)
            {
                case State.Idle:
                    yield return stateCoroutine = StartCoroutine(Idle());
                    break;
                case State.Generating:
                    yield return stateCoroutine = StartCoroutine(Generating());
                    break;
            }
        }
    }

    IEnumerator Idle()
    {
        while (state == State.Idle)
        {
            yield return null;
        }
    }

    // Falling Item을 랜덤으로 뿌립니다.
    IEnumerator Generating()
    {
        WaitForSeconds waitTime = new WaitForSeconds(Random.Range(averageTime - timeRange, averageTime + timeRange));
        while(state == State.Generating)
        {
            float xPos = Random.Range(-xRange, xRange);
            int probability = Random.Range(0, 100);

            GenerateItem(xPos, probability);
            yield return waitTime;
        }
    }

    void GenerateItem(float xPos, int prob)
    {
        int selectIndex = 0;
        for (int i = 0; i < itemList.itemList.Count; i++)
        {
            if (prob >= itemList.itemList[i].probability)
                continue;
            else
            {
                selectIndex = i;
                break;
            }
        }

        itemPoolingList[key].gameObject.SetActive(true);
        itemPoolingList[key].itemData = itemList.itemList[selectIndex].item.itemData;
        itemPoolingList[key].InitializeSetting();
        itemPoolingList[key].moveSpeed = Random.Range(averageSpeed - speedRange, averageSpeed + speedRange);
        itemPoolingList[key].transform.position = new Vector3(xPos, 7, 0);
        key = key + 1 < itemPoolingList.Count ? key + 1 : 0;
    }
}
