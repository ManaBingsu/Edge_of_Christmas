using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "ItemData", menuName = "Data/ItemData")]
public class ItemData : ScriptableObject
{
    [SerializeField]
    private int index; // 아이템 종류
    public int Index { get; set; }

    [SerializeField]
    private string script; // 아이템 설명
    public string Script { get; set; }

    [SerializeField]
    private Sprite icon; // 아이템 이미지
    public int Icon { get; set; }

    public enum CCState { idle, knockback, stun };
    [SerializeField]
    public CCState ccState;
}
