using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DummyPlayerParent : MonoBehaviour
{
    public delegate void GetItemEvent(DummyPlayerData playerData, DummyInventory inventoryData);
    public event GetItemEvent EvGetItem;

    public enum State { Idle, Walk, CC };
    public State state;
    protected Coroutine stateCoroutine;

    public bool isRage;

    //넉백 딜레이
    public bool isKnockBack;

    // 분노 시, 혹은 이벤트로 인한 보너스스피드
    [SerializeField]
    protected float bonusSpeed;

    [SerializeField]
    protected bool isGrounded;

    protected Coroutine knockBackCoroutine;
    protected Coroutine stunCoroutine;
    // 행동했을 때 마커 반짝반짝
    protected Coroutine markerCoroutine;

    [SerializeField]
    protected Color onColor;
    [SerializeField]
    protected Color offColor;

    protected Rigidbody2D rb2D;

    public enum Direction { left = -1, right = 1 };
    public Direction direction;
    // 보유한 플레이어 데이터
    public DummyPlayerData playerData;
    
    // 아이템 보유 공간
    public DummyInventory inventory;

    // 플레이어 애니메이터
    protected Animator anim;
    // 플레이어 렌더러
    protected SpriteRenderer sprRend;

    protected float originMoveSpeed;

    [Space(10)]
    [Header("Must Reference")]
    [SerializeField]
    protected SpriteRenderer markerSpr;
    // 분노 이펙트 오브젝트
    [SerializeField]
    protected GameObject rageFire;
    // 이펙트 애니메이터
    [SerializeField]
    protected Animator efcAnimator;
    // 아이템 리스트 참조
    [SerializeField]
    protected DummyItemList itemList;

    private void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            anim.SetBool("isJump", false);
        }

        if(col.gameObject.CompareTag("Player"))
        {

        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            anim.SetBool("isJump", true);
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
        /*
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
        }*/
    }

    private void OnCollisionStay2D(Collision2D col)
    {
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
            else
            {
                state = State.CC;
                if (knockBackCoroutine != null)
                    knockBackCoroutine = null;

                knockBackCoroutine = StartCoroutine(KnockBack((Direction)((int)direction * -1f), 0.05f, playerData.MoveSpeed * 2f));
            }
        }
    }

    protected virtual void Awake()
    {
        playerData = Instantiate(playerData) as DummyPlayerData;
        rb2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprRend = GetComponent<SpriteRenderer>();
        bonusSpeed = 1;
        originMoveSpeed = playerData.MoveSpeed;
    }

    protected virtual void Start()
    {
        // 나중에 바꿀 것
        StartCoroutine(ChargingRage());

        StartCoroutine(KeyManager());
        StartCoroutine(FSM());
        direction = (Direction)((int)playerData.team * -1);
        // 처음 방향전환
        sprRend.flipX = direction == Direction.left ? false : true;
    }

    protected virtual void Update()
    {
        if(transform.position.y < -5f)
        {
            if (BattleManager.battleManager.gameState != BattleManager.GameState.GameOver)
            {
                BattleManager.battleManager.gameState = BattleManager.GameState.GameOver;
                BattleManager.battleManager.winnerIndex = (int)playerData.team == -1 ? 1 : 0;
            }
            gameObject.SetActive(false);
        }
    }

    public void UseItem()
    {
        if (isRage)
            return;

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
                case State.Idle:
                    yield return stateCoroutine = StartCoroutine(Idle());
                    break;
                case State.Walk:
                    yield return stateCoroutine = StartCoroutine(Walk());
                    break;
                case State.CC:
                    yield return stateCoroutine = StartCoroutine(CC());
                    break;
            }
            yield return null;
        }
    }

    IEnumerator Idle()
    {
        while (state == State.Idle)
        {
            yield return null;
        }
    }

    IEnumerator Walk()
    {
        while (state == State.Walk)
        {
            Move();

            sprRend.flipX = direction == Direction.left ? false : true;

            anim.SetBool("isWalk", true);
            yield return null;
        }
        anim.SetBool("isWalk", false);
    }

    IEnumerator CC()
    {
        while (true)
        {
            if (knockBackCoroutine == null && stunCoroutine == null)
            {
                state = State.Walk;
                yield break;
            }
            yield return null;
        }
    }

    IEnumerator KeyManager()
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

                    if (Input.GetKeyDown(KeyCode.X))
                    {
                        UseItem();
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

                    if (Input.GetKeyDown(KeyCode.Period))
                    {
                        UseItem();
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
                playerData.Rage += 20;

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

    protected virtual IEnumerator KnockBack(Direction direction, float ccTime, float ccPower)
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

    protected virtual IEnumerator Stun(float ccTime)
    {
        // 분노모드 취소
        playerData.Rage -= 999;
        efcAnimator.gameObject.SetActive(true);

        float time = 0f;
        while(time < ccTime)
        {
            time += Time.deltaTime;

            yield return null;
        }
        efcAnimator.SetTrigger("TrgEnd");
        stunCoroutine = null;
    }

    protected IEnumerator ChargingRage()
    {
        WaitForSeconds waitTime = new WaitForSeconds(1.0f);
        while(true)
        {
            // 게임 진행 중 일때만 분노 참
            if(BattleManager.battleManager.gameState != BattleManager.GameState.Processing)
            {
                yield return null;
                continue;
            }

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
        if (state == State.Idle)
            return;

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
        if (state == State.CC)
            return;

        if(!isRage && playerData.Rage == playerData.MaxRage)
        {
            isRage = true;
            StartCoroutine(RageMode());
        }
    }

    protected IEnumerator DisplayMarker()
    {
        markerSpr.color = onColor;
        yield return new WaitForSeconds(0.25f);
        markerSpr.color = offColor;
        
    }

    protected IEnumerator RageMode()
    {
        bonusSpeed *= playerData.RageBonusSpeed;
        rageFire.SetActive(true);
        Debug.Log(rageFire.activeSelf);
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
        rageFire.SetActive(false);
        isRage = false;
        bonusSpeed /= playerData.RageBonusSpeed;
    }

    protected IEnumerator KnockBackDelay()
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
                Num++;
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
            else if(value > 5)
            {
                return;
            }
            num = value;
        }
    }
}
