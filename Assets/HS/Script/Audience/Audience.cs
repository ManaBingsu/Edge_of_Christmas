using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audience : MonoBehaviour
{
    public enum State { Idle, Angry, Cheer }
    public State state;

    public enum Direction { Left = -1, Right = 1 }
    public Direction direction;

    public enum CheerDirection { Down = -1, Up = 1 }
    public CheerDirection cheerDirection;

    public float leftX;
    public float rightX;

    private float upY;
    private float downY;

    [SerializeField]
    private float shakePower;

    [SerializeField]
    private float cheerPower;

    private void Awake()
    {
        leftX = transform.position.x - shakePower;
        rightX = transform.position.x + shakePower;
        upY = transform.position.y + cheerPower;
        downY = transform.position.y;
    }

    private void Update()
    {
        if (transform.position.x > rightX)
            direction = Direction.Left;
        else if (transform.position.x < leftX)
            direction = Direction.Right;

        if (transform.position.y > upY)
            cheerDirection = CheerDirection.Down;
        else if (transform.position.y < downY)
            cheerDirection = CheerDirection.Up;

    }
}
