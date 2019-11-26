using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audience : MonoBehaviour
{
    public enum Direction { Left = -1, Right = 1 }
    public Direction direction;

    public float leftX;
    public float rightX;

    [SerializeField]
    private float shakePower;

    private void Awake()
    {
        leftX = transform.position.x - shakePower;
        rightX = transform.position.x + shakePower;
    }

    private void Update()
    {
        if (transform.position.x > rightX)
            direction = Direction.Left;
        else if (transform.position.x < leftX)
            direction = Direction.Right;
    }
}
