using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyPlayerGeneratorManager : MonoBehaviour
{
    public List<DummyPlayerGenerator> playerGeneratorList;

    public List<DummyPlayerSkinData> skinDataList;

    private void Awake()
    {
        GeneratePlayer();
    }

    public void GeneratePlayer()
    {
        for(int i = 0; i < 2; i++)
        {
            playerGeneratorList[i].GeneratePlayer(i);
        }
    }
}
