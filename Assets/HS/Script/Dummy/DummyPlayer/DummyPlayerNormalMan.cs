using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyPlayerNormalMan : DummyPlayerParent
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            playerData.Gift++;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerData.Gift--;
        }
    }
}
