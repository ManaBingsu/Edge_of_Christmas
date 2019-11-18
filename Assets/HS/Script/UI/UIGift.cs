using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGift : MonoBehaviour
{
    [Header("Reference")]
    private Image giftImg;

    [Header("Value")]
    // 전광판 켜는 코루틴
    private Coroutine onOffCoroutine;
    [SerializeField]
    private float colorChangeSpeed;
    // 전광판 꺼졌을 때 색상
    [SerializeField]
    private Color offColor;
    // 전광판 켜졌을 때 색상
    [SerializeField]
    private Color onColor;

    // 전광판 온 오프!
    [SerializeField]
    private bool isOn;
    public bool IsOn
    {
        get { return isOn; }
        set
        {
            isOn = value;

            if (onOffCoroutine != null)
                StopCoroutine(onOffCoroutine);

            if (isOn)
            {
                onOffCoroutine = StartCoroutine(OnGift());
            }
            else
            {
                onOffCoroutine = StartCoroutine(OffGift());
            }
        }
    }

    private void Awake()
    {
        giftImg = GetComponent<Image>();
        // 초기 색 지정 - 꺼진 색
        giftImg.color = offColor;
    }

    // 선물을 얻음
    IEnumerator OnGift()
    {
        float progress = 0;
        Color currentColor = giftImg.color;
        while(progress < 1)
        {
            progress += Time.deltaTime * colorChangeSpeed;

            giftImg.color = Color.Lerp(currentColor, onColor, progress);

            yield return null;
        }
    }
    // 선물을 잃음
    IEnumerator OffGift()
    {
        float progress = 0;
        Color currentColor = giftImg.color;
        while (progress < 1)
        {
            progress += Time.deltaTime * colorChangeSpeed;

            giftImg.color = Color.Lerp(currentColor, offColor, progress);

            yield return null;
        }
    }
}
