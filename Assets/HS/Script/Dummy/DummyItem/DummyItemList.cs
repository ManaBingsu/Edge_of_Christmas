using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyItemList : MonoBehaviour
{
    public List<ItemDictionary> itemList;
}

[System.Serializable]
public struct ItemDictionary
{
    public DummyItemParent item;
    public float probability;
}
