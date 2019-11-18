using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DummyItemParent : MonoBehaviour
{
    public DummyItemData itemData;

    public abstract void Activate(DummyPlayerParent player);
}
