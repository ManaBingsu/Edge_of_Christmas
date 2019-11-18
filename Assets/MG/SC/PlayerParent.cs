using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParent : MonoBehaviour
{
    private Inventory inventory;
    public PlayerData playerData;
    private Rigidbody2D rb2D;
    Vector3 movement;
    List<ItemParent> itemList;

    //플레이어의 상태
    enum State { idle, walk, walk_Jump, rage, CC}
    State state;
    // 보고있는 방향
    public enum Direction { left, right };
    public Direction direction;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();

        // 좌측팀이면 오른쪽으로, 우측팀이면 왼쪽으로 이동
        if (playerData.team == PlayerData.Team.Left) 
        {
            movement.Set(1, 0, 0);
            direction = Direction.right;
        }
        else
        {
            movement.Set(-1, 0, 0);
            direction = Direction.left;
        }
        movement = movement.normalized * playerData.MoveSpeed  * Time.deltaTime;
        // 플레이어의 기본 상태는  walk
        state = State.walk;
    }
    private void Update()
    {
        DirChange();
        // 플레이어가 걷는상태일 때 스페이스를 누르면 점프가능
        if (Input.GetKeyDown(KeyCode.Space) && state == State.walk)
            PlayerJump();
        // 아이템 던지기
        if (Input.GetKeyDown(KeyCode.Z))
            ItemThrow();
    }
    private void FixedUpdate()
    {
        PlayerMove();
    }
    void DirChange()
    {
        // 탭누르면 방향전환
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            movement = -movement;
            // 보고있는방향의 정보 바꾸기
            if (direction == Direction.left)
                direction = Direction.right;
            else
                direction = Direction.left;
        }
    }
    void PlayerMove()
    {
        transform.Translate(movement); // 리지드바디 사용하니까 이상해서 일단 보류해둠 ㅜ.ㅜ
     //   rb2D.MovePosition(transform.position + movement);
    }
    void PlayerJump()
    {
        // 점프점프
        state = State.walk_Jump;
        Vector2 jumpPower = new Vector2(0, playerData.JumpPower);
        rb2D.AddForce(jumpPower, ForceMode2D.Impulse);
    }
    void ItemThrow()
    {
        // 보고있는 방향에 따라 아이템 투척
        if (direction == Direction.left)
            inventory.MyItem.Activate(Direction.left);
        else
            inventory.MyItem.Activate(Direction.right);
    }

    void GetItem(int _index)
    {
        inventory.MyItem = itemList[_index];
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 내가 밟은게 땅이나 플레이어면 점프 가능
        if (collision.tag == "Ground" || collision.tag == "Player")
        {
            state = State.walk;
        }
    }
}

struct Inventory
{

    private ItemParent myItem; 
    public ItemParent MyItem
    {
        get { return myItem; }
        set
        {
            // 만약 다른 아이템이 들어올 경우 아이템 개수와 종류를 초기화시킨다.
            if (myItem.itemData.Index != value.itemData.Index)
            {
                num = 1;
                myItem = value;
            }
            else // 같은 아이템이면 개수추가
            {
                num++;
            }
        }
    }

    private int num;
    public int Num { get; set; }
}