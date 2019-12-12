using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEnding : MonoBehaviour
{
    public Image oneWin;
    public Image twoWin;
    public Image giftWin;

    public void SetWinImage()
    {
        if(DummySystemManager.systemManager.playerList[0].playerData.Gift == 5 || DummySystemManager.systemManager.playerList[1].playerData.Gift == 5)
        {
            // 큰 차이로 이김
            if (Mathf.Abs(DummySystemManager.systemManager.playerList[0].playerData.Gift - DummySystemManager.systemManager.playerList[1].playerData.Gift) > 1)
            {
                GiftWin();
            }
            // 근소한 차이로 이김
            else
            {
                OnePlayerWin();
            }
        }
        // 한명 낙사
        else
        {
            TwoPlayerLose();
        }

    }

    // 한명이 완전히 이김
    public void OnePlayerWin()
    {
        oneWin.gameObject.SetActive(true);
        twoWin.gameObject.SetActive(false);
        giftWin.gameObject.SetActive(false);
    }

    // 낙사 또는 근소한 차이로 이김
    public void TwoPlayerLose()
    {
        oneWin.gameObject.SetActive(false);
        twoWin.gameObject.SetActive(true);
        giftWin.gameObject.SetActive(false);
    }

    // 선물 그림 예뻐서 씀
    public void GiftWin()
    {
        oneWin.gameObject.SetActive(false);
        twoWin.gameObject.SetActive(false);
        giftWin.gameObject.SetActive(true);
    }
}
