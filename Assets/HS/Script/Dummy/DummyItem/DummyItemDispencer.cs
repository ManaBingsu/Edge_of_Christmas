using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyItemDispencer : MonoBehaviour
{
    private int key;

    [SerializeField]
    private List<DummyItemFlying> itemList;

    private void Awake()
    {
        SetItemList();
    }

    void SetItemList()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            itemList.Add(transform.GetChild(i).GetComponent<DummyItemFlying>());
        }
    }

    public void Shot(DummyPlayerParent player, DummyItemData data)
    {
        itemList[key].gameObject.SetActive(true);
        itemList[key].InitializeSetting(player, data);

        key = key + 1 < itemList.Count ? key + 1 : 0;

    }
}
