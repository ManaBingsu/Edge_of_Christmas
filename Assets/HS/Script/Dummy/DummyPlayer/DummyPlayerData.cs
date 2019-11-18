using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DummyPlayerData", menuName = "DummyData/DummyPlayerData")]
public class DummyPlayerData : ScriptableObject
{
    public delegate void TeamEventHandler(Team t);
    public event TeamEventHandler EvGift;
    public delegate void EventHandler();

    public enum Team { Left = -1, Right = 1 }
    public Team team;

    [SerializeField]
    private int maxGift;

    [SerializeField]
    private int gift;
    public int Gift
    {
        get { return gift; }
        set
        {
            if (value < 0)
            {
                rage = 0;
                return;
            }

            if (value > maxGift)
                return;

            gift = value;
            EvGift?.Invoke(team);
        }
    }

    public event EventHandler EvRage;
    [SerializeField]
    private int rage;
    public int Rage
    {
        get { return rage; }
        set
        {
            if(value < 0)
            {
                rage = 0;
                EvRage?.Invoke();
                return;
            }
            else if(value > MaxRage)
            {
                rage = MaxRage;
                EvRage?.Invoke();
                return;
            }
            rage = value;
            EvRage?.Invoke();
        }
    }

    [SerializeField]
    private int maxRage;
    public int MaxRage { get { return maxRage; } set { maxRage = value; } }

    [SerializeField]
    private int rageChargingPoint;
    public int RageChargingPoint { get { return rageChargingPoint; } set { rageChargingPoint = value; } }

    [SerializeField]
    private float moveSpeed;
    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; }}

    [SerializeField]
    private float rageBonusSpeed;
    public float RageBonusSpeed { get { return rageBonusSpeed; } set { rageBonusSpeed = value; } }

    [SerializeField]
    private float ragePower;
    public float RagePower { get { return ragePower; } set { ragePower = value; } }

    [SerializeField]
    private float ragePowerTime;
    public float RagePowerTime { get { return ragePowerTime; } set { ragePowerTime = value; } }

    [SerializeField]
    private float jumpPower;
    public float JumpPower { get { return jumpPower; } set { jumpPower = value; } }
}
