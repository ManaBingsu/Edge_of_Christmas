using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DummyItemData", menuName = "DummyData/DummyItemData")]
public class DummyItemData : ScriptableObject
{
    // 투사체 인지, 강화형인지
    public enum Type { Throwing, Reinforce, Gift }
    // 보유 CC 종류
    public enum CrowdControl { None, KnockBack, Stun }
    [Header("State")]   
    public Type type;
    public CrowdControl crowdControl;

    [Header("Information")]
    [SerializeField]
    private int index;
    public int Index { get { return index; } set { index = value; } }

    [TextArea]
    [SerializeField]
    private string script;
    public string Script { get { return script; } set { script = value; } }

    [SerializeField]
    private Sprite icon;
    public Sprite Icon { get { return icon; } set { icon = value; } }

    [Header("Value")]
    [SerializeField]
    private float moveSpeed;
    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }
    [SerializeField]
    private float ccTime;
    public float CCTime { get { return ccTime; } set { ccTime = value; } }
    [SerializeField]
    private float ccPower;
    public float CCPower { get { return ccPower; } set { ccPower = value; } }
}
