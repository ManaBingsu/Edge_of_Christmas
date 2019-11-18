using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "PlayerData", menuName = "Data/PlayerData")]
public class PlayerData : ScriptableObject
{
    public enum Team { Left, Right };

    [SerializeField]
    public Team team;
    
    public delegate void EventHandler();
    public event EventHandler EvGift;

    [SerializeField]
    private int gift; // 선물게이지
    public int Gift // 선물통제
    {
        get { return gift; }
        set
        {
            gift = value;
            EvGift?.Invoke();
        }
    }

    [SerializeField]
    private int rage; // 각성게이지
    public int Rage
    {
        get { return rage; }
        set
        {
            rage = value;
        }
    }

    [SerializeField]
    private int maxRage; // 최대각성게이지
    public int MaxRage { get; set; }

    [SerializeField]
    private float moveSpeed; // 플레이어 이동속도
    public float MoveSpeed
    {
        get { return moveSpeed; }
        set
        {
            moveSpeed = value;
        }
    }
    [SerializeField]
    private float jumpPower; // 플레이어 점프력
    public float JumpPower
    {
        get { return jumpPower; }
        set
        {
            jumpPower = value;
        }
    }
}
