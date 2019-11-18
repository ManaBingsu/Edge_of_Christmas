using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSnowball : ItemParent
{
    public override void Activate(PlayerParent.Direction direction)
    {
        Debug.Log("1");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(this);
    }
}
