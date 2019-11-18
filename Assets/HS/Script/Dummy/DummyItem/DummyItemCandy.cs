﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyItemCandy : DummyItemParent
{
    [SerializeField]
    private DummyItemDispencer itemDispencer;

    public override void Activate(DummyPlayerParent player)
    {
        itemDispencer.Shot(player, itemData);
    }
}
