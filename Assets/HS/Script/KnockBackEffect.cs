using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackEffect : MonoBehaviour
{
    public static KnockBackEffect kbEffect;

    [SerializeField]
    private List<KnockBackStar> starList;

    private int key;

    private void Awake()
    {
        if (kbEffect == null)
            kbEffect = this;

        for(int i = 0; i < transform.childCount; i++)
        {
            starList.Add(transform.GetChild(i).GetComponent<KnockBackStar>());
        }
    }

    public void EmitStarEffect(int playerIndex, int dir)
    {
        int randomNum = Random.Range(2, 4);
        for(int i = 0; i < randomNum; i++)
        {
            ShotStar(playerIndex, dir);
        }
    }

    void ShotStar(int playerIndex, int dir)
    {
        starList[key].gameObject.SetActive(true);
        starList[key].transform.position = new Vector3(
            DummySystemManager.systemManager.playerList[playerIndex > 0 ? 1 : 0].transform.position.x,
             DummySystemManager.systemManager.playerList[playerIndex > 0 ? 1 : 0].transform.position.y + 0.5f,
             starList[key].transform.position.z);
        // 별 크기 랜덤 지정
        float randomSize = Random.Range(0.6f, 0.9f);
        starList[key].transform.localScale = new Vector3(randomSize, randomSize, 1f);

        float Radian = 0;
        if (dir == -1)
        {
            Radian = Random.Range(110, 150) * Mathf.Deg2Rad;
        }
        else
        {
            Radian = Random.Range(70, 30) * Mathf.Deg2Rad;
        }
        Vector3 shotDir = new Vector3(Mathf.Cos(Radian), Mathf.Sin(Radian), 0f);
        starList[key].direction = shotDir.normalized;

        key = key + 1 < starList.Count ? key + 1 : 0;
    }
}
