using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public enum CheerDirection { Down = -1, Up = 1 }
    public CheerDirection cheerDirection;

    public float upY;
    public float downY;

    public static CameraControl camControl;

    private Vector3 originPos;

    public Coroutine shakeCoroutine;

    private void Awake()
    {
        if (camControl == null)
            camControl = this;

        originPos = transform.position;
        upY = transform.position.y + 0.2f;
        downY = transform.position.y - 0.2f;

        cheerDirection = CheerDirection.Up;
    }

    private void Update()
    {
        if (transform.position.y > upY)
            cheerDirection = CheerDirection.Down;
        else if (transform.position.y < downY)
            cheerDirection = CheerDirection.Up;
    }

    public IEnumerator Shake(float _amount, float _duration)
    {
        float timer = 0;
        while (timer <= _duration)
        {
            transform.position += Vector3.up * (int)cheerDirection * _amount * Time.deltaTime;

            timer += Time.deltaTime;
            yield return null;
        }
        transform.position = originPos;
    }
}
