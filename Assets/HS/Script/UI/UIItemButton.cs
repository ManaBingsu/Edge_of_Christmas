using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIItemButton : MonoBehaviour
{
    public int playerIndex;
    public DummyPlayerParent player;
    public DummyItemParent item;

    private void Start()
    {
        player = DummySystemManager.systemManager.playerList[playerIndex];
    }

    public void ClickButton()
    {
        player.UseItem();
    }

}
