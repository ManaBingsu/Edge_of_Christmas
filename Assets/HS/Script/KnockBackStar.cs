using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackStar : MonoBehaviour
{
    private SpriteRenderer sprRend;
    private Color originColor;
    private Color blankColor;

    public Vector3 direction;

    public AnimationCurve speedCurve;

    private void Awake()
    {
        sprRend = GetComponent<SpriteRenderer>();
        originColor = sprRend.color;
        blankColor = new Color(originColor.r, originColor.g, originColor.b, 0);
    }

    private void OnEnable()
    {
        sprRend.color = originColor;
        StartCoroutine(StartEffect());
    }

    IEnumerator StartEffect()
    {
        float process = 0f;

        // 별 속도 랜덤 지정
        float speed = Random.Range(3f, 4.5f);

        // 별 각도 랜덤 지정
        float rot = Random.Range(0, 360);
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, rot);

        while(process < 1f)
        {
            process += Time.deltaTime * 3f;

            sprRend.color = Color.Lerp(originColor, blankColor, process);

            transform.position = Vector3.Lerp(transform.position, transform.position + direction, speedCurve.Evaluate(speed * Time.deltaTime));

            yield return null;
        }

        gameObject.SetActive(false);
    }
}

