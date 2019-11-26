using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerSkin : MonoBehaviour
{
    // 현재 선택된 스킨 index목록

    public PlayerSkin[] skinIconUI = new PlayerSkin[2];
    public GameObject[] skinInfoArray = new GameObject[2];
    public int[] key = new int[2];

    public List<DummyPlayerSkinData> skinData;

    private void Start()
    {
        for(int i = 0; i < 2; i++)
        {
            skinIconUI[i].index = skinData[0].Index;
            skinIconUI[i].story.text = skinData[0].Story;
            skinIconUI[i].abilityScript.text = skinData[0].AbilityScript;
            skinIconUI[i].icon.sprite = skinData[0].Icon;
            skinIconUI[i].skinName.text = skinData[0].SkinName;
        }
    }

    public void UpLeftSkinIndex()
    {
        SetSkinIndex(0, true);
    }

    public void DownLeftSkinIndex()
    {
        SetSkinIndex(0, false);
    }

    public void UpRightSkinIndex()
    {
        SetSkinIndex(1, true);
    }

    public void DownRightSkinIndex()
    {
        SetSkinIndex(1, false);
    }

    public void ClickLeftSkinInfo()
    {
        bool isOn = skinInfoArray[0].activeSelf ? false : true;
        skinInfoArray[0].SetActive(isOn);
    }

    public void ClickRightSkinInfo()
    {
        bool isOn = skinInfoArray[1].activeSelf ? false : true;
        skinInfoArray[1].SetActive(isOn);
    }

    public void SetSkinIndex(int index, bool isUp)
    {
        if (isUp)
            key[index] = key[index] + 1 > skinData.Count - 1 ? 0 : key[index] + 1;
        else
            key[index] = key[index] - 1 < 0 ? skinData.Count - 1 : key[index] - 1;

        skinIconUI[index].index = skinData[key[index]].Index;
        skinIconUI[index].story.text = skinData[key[index]].Story;
        skinIconUI[index].abilityScript.text = skinData[key[index]].AbilityScript;
        skinIconUI[index].icon.sprite = skinData[key[index]].Icon;
        skinIconUI[index].skinName.text = skinData[key[index]].SkinName;

        // 시스템에 동기화
        DummySystemManager.systemManager.playerSkinIndexArray[index] = key[index];
    }

    public void StartGame()
    {
        for(int i = 0; i < 2; i++)
        {
            DummySystemManager.systemManager.playerSkinIndexArray[i] = 0;
        }
    }
}

[System.Serializable]
public class PlayerSkin
{
    public int index;
    public Image icon;
    public Text skinName;
    public Text story;
    public Text abilityScript;
}
