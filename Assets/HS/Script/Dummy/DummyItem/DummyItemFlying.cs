using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyItemFlying : MonoBehaviour
{
    public enum State { Flying, Die }
    public State state;
    
    // 누가 발사했는지
    public DummyPlayerData.Team team;
    // 발사한 사람의 방향은
    public DummyPlayerParent.Direction direction;

    Coroutine fsmCoroutine;
    [SerializeField]
    private float dieTime;

    Vector3 initialPos;

    public DummyItemData itemData;

    private SpriteRenderer sprRend;
    private List<Sprite> sprList;
    private Animator efcObj;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("FlyingItem"))
        {
            if (state != State.Die)
            {
                state = State.Die;
            }
        }     
    }

    private void Awake()
    {
        sprRend = GetComponent<SpriteRenderer>();
        efcObj = transform.GetChild(0).GetComponent<Animator>();
    }

    IEnumerator FSM()
    {
        while (true)
        {
            switch(state)
            {
                case State.Flying:
                    Flying();
                    break;
                case State.Die:
                    StartCoroutine(Die());
                    yield break;
            }
            yield return null;
        }
    }

    public void InitializeSetting(DummyPlayerParent player, DummyItemData data)
    {
        this.team = player.playerData.team;
        this.direction = player.direction;
        this.itemData = data;
        initialPos = new Vector2(player.transform.position.x, player.transform.position.y + 0.5f);
        sprRend.sprite = data.Icon;
        transform.position = initialPos;
        state = State.Flying;
        fsmCoroutine = StartCoroutine(FSM());
    }

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }

    void Flying()
    {
        transform.position += Vector3.right * itemData.MoveSpeed * (int)direction * Time.deltaTime;
        transform.Rotate(Vector3.forward, itemData.MoveSpeed * (int)direction * -1);
    }

    IEnumerator Die()
    {
        float time = 0f;
        if (itemData.Index == 0)
        {
            sprRend.sprite = null;
            efcObj.gameObject.SetActive(true);
        }

        while (time < dieTime)
        {
            // 캔디
            if(itemData.Index == 1)
            {
                transform.position += (Vector3.right * (int)direction * itemData.MoveSpeed / 2f + Vector3.down * (itemData.MoveSpeed + time * 2f)) * Time.deltaTime;
                transform.Rotate(Vector3.forward, itemData.MoveSpeed * (int)direction * -1 * 0.5f);
            }
            time += Time.deltaTime;
            // 터지는 애니메이션 발동
            yield return null;
        }
        efcObj.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
