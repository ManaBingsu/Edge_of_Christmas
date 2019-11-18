using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemParent : MonoBehaviour
{
    [SerializeField]
    public ItemData itemData;

    public abstract void Activate(PlayerParent.Direction direction);

}
