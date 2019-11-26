using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemControl : MonoBehaviour
{
    [SerializeField]
    private Button[] itemButtonArray = new Button[2];

    [SerializeField]
    private Text[] itemCountArray = new Text[2];

    [SerializeField]
    private Sprite nullSpr;

    private void Start()
    {
        for (int i = 0; i < 2; i++)
            DummySystemManager.systemManager.playerList[i].EvGetItem += new DummyPlayerParent.GetItemEvent(SetItemImage);
    }

    void SetItemImage(DummyPlayerData playerData, DummyInventory item)
    {
        int targetIndex = playerData.team > 0 ? 1 : 0;
        if(item.MyItem == null)
        {
            itemButtonArray[targetIndex].image.sprite = nullSpr;
            itemCountArray[targetIndex].text = "";
        }
        else
        {
            itemButtonArray[targetIndex].image.sprite = item.MyItem.itemData.Icon;
            itemCountArray[targetIndex].text = item.Num.ToString();
        }
    }
}

