using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRage : MonoBehaviour
{
    [SerializeField]
    private int playerIndex;

    [SerializeField]
    private Image gaugeImage;

    [SerializeField]
    private Image playerImage;

    private void Start()
    {
        DummySystemManager.systemManager.playerList[playerIndex].playerData.EvRage += new DummyPlayerData.EventHandler(SetGauge);
        SetGauge();
    }

    public void ClickRage()
    {
        DummySystemManager.systemManager.playerList[playerIndex].Rage();
    }

    void SetGauge()
    {
        gaugeImage.fillAmount = ((float)DummySystemManager.systemManager.playerList[playerIndex].playerData.Rage / DummySystemManager.systemManager.playerList[playerIndex].playerData.MaxRage);
    }
}
