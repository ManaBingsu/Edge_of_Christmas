using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyItemFalling : MonoBehaviour
{
    public enum State { Falling, Die }
    public State state;

    private SpriteRenderer sprRend;

    // 평균 값
    public float moveSpeed;

    public DummyItemData itemData;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Ground"))
        {
            gameObject.SetActive(false);
        }
    }

    private void Awake()
    {
        sprRend = GetComponent<SpriteRenderer>();
    }

    public void InitializeSetting()
    {
        sprRend.sprite = itemData.Icon;
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(-70, 70));
    }

    private void OnEnable()
    {
        state = State.Falling;
        StartCoroutine(Falling());
    }

    IEnumerator Falling()
    {
        while(true)
        {
            transform.position += Vector3.down * moveSpeed * Time.deltaTime;
            yield return null;
        }
    }
}
