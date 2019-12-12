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

    private Coroutine shiningCoroutine;
    private Color originColor;
    [SerializeField]
    private Color shiningColor;

    private void Awake()
    {
        originColor = gaugeImage.color;
    }

    private void Start()
    {
        DummySystemManager.systemManager.playerList[playerIndex].playerData.EvRage += new DummyPlayerData.EventHandler(SetGauge);
        SetGauge();
    }

    public void ClickRage()
    {
        if (BattleManager.battleManager.gameState != BattleManager.GameState.Processing)
            return;
        if (shiningCoroutine != null)
            StopCoroutine(shiningCoroutine);
        gaugeImage.color = originColor;
        DummySystemManager.systemManager.playerList[playerIndex].Rage();
    }

    void SetGauge()
    {
        gaugeImage.fillAmount = ((float)DummySystemManager.systemManager.playerList[playerIndex].playerData.Rage / DummySystemManager.systemManager.playerList[playerIndex].playerData.MaxRage);
        if(gaugeImage.fillAmount >= 1f && shiningCoroutine == null)
        {
            shiningCoroutine = StartCoroutine(Shining());
        }
    }

    IEnumerator Shining()
    {
        WaitForSeconds waitTime = new WaitForSeconds(0.2f);
        while(gaugeImage.fillAmount >= 1.0f)
        {
            gaugeImage.color = shiningColor;
            yield return waitTime;
            gaugeImage.color = originColor;
            yield return waitTime;
        }
    }

}
