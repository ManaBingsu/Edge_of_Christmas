using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGiftControl : MonoBehaviour
{
    // 양쪽 선물을 담고 있음, 0 : 왼쪽 1 : 오른쪽
    [SerializeField]
    private GiftDisplayer[] giftArray = new GiftDisplayer[2];

    private void Start()
    {
        for(int i = 0; i < 2; i++)
        {
            DummySystemManager.systemManager.playerList[i].playerData.EvGift += new DummyPlayerData.TeamEventHandler(DisplayGift);
        }
    }

    // Gift 상태 반영
    void DisplayGift(DummyPlayerData.Team team)
    {
        int teamIndex = (int)team > 0 ? 1 : 0;
        // Gift 점수를 SystemManager의 Player로 부터 받아온다
        int gift = DummySystemManager.systemManager.playerList[teamIndex].playerData.Gift;
        // 현재 Gift가 On 되어야 하는 상태인가
        bool isDisplayOn = giftArray[teamIndex].endKey < gift ? true : false;

        for(int i = 0; i < 5; i++)
        {
            if(i < gift)
                giftArray[teamIndex].giftList[i].IsOn = true;
            else
                giftArray[teamIndex].giftList[i].IsOn = false;
        }
    }
}

[System.Serializable]
struct GiftDisplayer
{
    public List<UIGift> giftList;
    // 마지막 선물의 인덱스를 지칭
    public int endKey;
}
