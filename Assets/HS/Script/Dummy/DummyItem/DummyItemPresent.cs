using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyItemPresent : DummyItemParent
{
    [SerializeField]
    private DummyItemDispencer itemDispencer;

    public override void Activate(DummyPlayerParent player) { }
}
