using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DummyPlayerParent : MonoBehaviour
{
    public delegate void GetItemEvent(DummyPlayerData playerData, DummyInventory inventoryData);
    public event GetItemEvent EvGetItem;

    public enum State { idle, walk, CC };
    public State state;

    public bool isRage;

    //넉백 딜레이
    public bool isKnockBack;

    // 분노 시, 혹은 이벤트로 인한 보너스스피드
    [SerializeField]
    private float bonusSpeed;

    [SerializeField]
    private bool isGrounded;

    Coroutine knockBackCoroutine;
    Coroutine stunCoroutine;
    // 행동했을 때 마커 반짝반짝
    Coroutine markerCoroutine;
    [SerializeField]
    private SpriteRenderer markerSpr;
    [SerializeField]
    private Color onColor;
    [SerializeField]
    private Color offColor;

    [SerializeField]
    private Rigidbody2D rb2D;

    public enum Direction { left = -1, right = 1 };
    public Direction direction;
    // 보유한 플레이어 데이터
    public DummyPlayerData playerData;
    
    // 아이템 보유 공간
    public DummyInventory inventory;

    // 아이템 리스트 참조
    [SerializeField]
    private DummyItemList itemList;

    private void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("FlyingItem"))
        {
            DummyItemFlying item = col.gameObject.GetComponent<DummyItemFlying>();
            if (item.team != playerData.team && item.state == DummyItemFlying.State.Flying)
            {
                item.state = DummyItemFlying.State.Die;
                GetDamage(item);
            }

        }

        if (col.gameObject.CompareTag("FallingItem"))
        {
            DummyItemFalling item = col.gameObject.GetComponent<DummyItemFalling>();

            if (item.itemData.type == DummyItemData.Type.Gift)
            {
                playerData.Gift += (int)item.itemData.CCPower;
            }
            else if (item.itemData.type == DummyItemData.Type.Throwing)
            {
                GetItem(item.itemData.Index);
            }
            item.gameObject.SetActive(false);
        }

        if (col.gameObject.CompareTag("Player"))
        {
            DummyPlayerParent player = col.gameObject.GetComponent<DummyPlayerParent>();

            if (player.playerData.team != playerData.team && player.isRage && player.transform.position.y < transform.position.y + 0.25f)
            {
                state = State.CC;
                if (knockBackCoroutine != null)
                    knockBackCoroutine = null;
                knockBackCoroutine = StartCoroutine(KnockBack(player.direction, player.playerData.RagePowerTime, player.playerData.RagePower));
            }
        }
    }

    void OnEnable()
    {
        state = State.walk;
        StartCoroutine(StateManager());
        StartCoroutine(FSM());
        direction = (Direction)((int)playerData.team * -1);
    }

    protected virtual void Awake()
    {
        playerData = Instantiate(playerData) as DummyPlayerData;
        rb2D = GetComponent<Rigidbody2D>();
        markerSpr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        bonusSpeed = 1;
    }

    private void Start()
    {
        // 나중에 바꿀 것
        StartCoroutine(ChargingRage());
    }

    public void UseItem()
    {
        if (inventory.MyItem == null)
            return;

        inventory.MyItem.Activate(this);
        inventory.Num--;
        EvGetItem?.Invoke(playerData, inventory);
    }

    public void GetItem(int idx)
    {
        inventory.MyItem = itemList.itemList[idx].item;
        // 관련 UI 이벤트 발생
        EvGetItem?.Invoke(playerData, inventory);
    }

    IEnumerator FSM()
    {
        while(true)
        {
            switch(state)
            {
                case State.idle:

                    break;
                case State.walk:
                    Move();
                    break;
                case State.CC:
                    if (knockBackCoroutine == null && stunCoroutine == null)
                    {
                        state = State.walk;
                    }
                    break;
            }
            yield return null;
        }
    }

    IEnumerator StateManager()
    {
        while(true)
        {
            if (state != State.CC)
            {
                if (playerData.team == DummyPlayerData.Team.Left)
                {
                    if (Input.GetKeyDown(KeyCode.LeftShift))
                    {
                        ChangeDirection();
                    }

                    if (Input.GetKeyDown(KeyCode.Z))
                    {
                        Jump();
                    }
                }
                else if (playerData.team == DummyPlayerData.Team.Right)
                {
                    if (Input.GetKeyDown(KeyCode.RightShift))
                    {
                        ChangeDirection();
                    }

                    if (Input.GetKeyDown(KeyCode.Slash))
                    {
                        Jump();
                    }
                }
            }
            yield return null;
        }
    }

    public virtual void GetDamage(DummyItemFlying data)
    {
        // 만약 아이템에 cc가 있을 경우
        if(data.itemData.crowdControl != DummyItemData.CrowdControl.None)
        {
            state = State.CC;
            if (data.itemData.crowdControl == DummyItemData.CrowdControl.KnockBack)
            {
                if (knockBackCoroutine != null)
                    knockBackCoroutine = null;
                knockBackCoroutine = StartCoroutine(KnockBack(data.direction, data.itemData.CCTime, data.itemData.CCPower));
            }
            else if(data.itemData.crowdControl == DummyItemData.CrowdControl.Stun)
            {
                if (stunCoroutine != null)
                    stunCoroutine = null;
                stunCoroutine = StartCoroutine(Stun(data.itemData.CCTime));
            }
        }
    }

    IEnumerator KnockBack(Direction direction, float ccTime, float ccPower)
    {
        if (isKnockBack)
            yield break;

        StartCoroutine(KnockBackDelay());

        rb2D.velocity = new Vector2(0f, rb2D.velocity.y);
        rb2D.AddForce((Vector2.right * (int)direction + Vector2.up).normalized * ccPower, ForceMode2D.Impulse);
        float time = 0f;
        while (time < ccTime)
        {
            time += Time.deltaTime;

            yield return null;
        }
        yield return null;
        knockBackCoroutine = null;
    }

    IEnumerator Stun(float ccTime)
    {
        // 분노모드 취소
        playerData.Rage -= 999;

        float time = 0f;
        while(time < ccTime)
        {
            time += Time.deltaTime;

            yield return null;
        }
        stunCoroutine = null;
    }

    IEnumerator ChargingRage()
    {
        WaitForSeconds waitTime = new WaitForSeconds(1.0f);
        while(true)
        {
            if (!isRage)
                playerData.Rage += playerData.RageChargingPoint;
            yield return waitTime;
        }
    }

    public void ChangeDirection()
    {
        if (markerCoroutine != null)
            StopCoroutine(markerCoroutine);
        markerCoroutine = StartCoroutine(DisplayMarker());

        direction = (Direction)((int)direction * -1);
    }

    void Move()
    {
        rb2D.position += Vector2.right * (playerData.MoveSpeed * bonusSpeed)* (int)direction * Time.deltaTime;
    }

    public void Jump()
    {

        if (!isGrounded)
            return;

        if (isRage)
            return;

        if (markerCoroutine != null)
          StopCoroutine(markerCoroutine);
        markerCoroutine = StartCoroutine(DisplayMarker());
        
        if (markerCoroutine != null)
            StopCoroutine(markerCoroutine);
        markerCoroutine = StartCoroutine(DisplayMarker());

        rb2D.AddForce(Vector2.up * playerData.JumpPower, ForceMode2D.Impulse);
    }

    public void Rage()
    {
        if(!isRage && playerData.Rage == playerData.MaxRage)
        {
            isRage = true;
            StartCoroutine(RageMode());
        }
    }

    IEnumerator DisplayMarker()
    {
        markerSpr.color = onColor;
        yield return new WaitForSeconds(0.25f);
        markerSpr.color = offColor;
        
    }

    IEnumerator RageMode()
    {
        bonusSpeed *= playerData.RageBonusSpeed;

        float time = 0f;

        while (playerData.Rage > 0)
        {
            time += Time.deltaTime;
            if(time > 0.1f)
            {
                time = 0f;
                playerData.Rage--;
            }
            yield return null;
        }
        isRage = false;
        bonusSpeed /= playerData.RageBonusSpeed;
    }

    IEnumerator KnockBackDelay()
    {
        isKnockBack = true;
        yield return new WaitForSeconds(0.2f);
        isKnockBack = false;
    }
}

public struct DummyInventory
{
    private DummyItemParent myItem;
    public DummyItemParent MyItem
    {
        get { return myItem; }
        set
        {
            if(value == null)
            {
                myItem = null;
            }
            if(myItem == null)
            {
                num = 1;
                myItem = value;
                return;
            }
            // 만약 다른 아이템이 들어올 경우 아이템 개수와 종류를 초기화시킨다.
            if(myItem.itemData.Index != value.itemData.Index)
            {
                num = 1;
                myItem = value;
            }
            else
            {
                num++;
            }
        }
    }

    private int num;
    public int Num {
        get
        {
            return num;
        }
        set
        {
            if(value < 1)
            {
                num = 0;
                MyItem = null;
                return;
            }
            num = value;
        }
    }
}
