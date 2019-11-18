using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummySystemManager : MonoBehaviour
{
    // 게임 상태 관리
    public enum GameState { BeforeStart, Processing, GameOver }

    public static DummySystemManager systemManager;

    // 현재 선택된 플레이어 스킨의 인덱스 정보, 로비 창에서 스킨을 선택했을 때 이 변수를 바꿔주면 된다
    public int[] playerSkinIndexArray = new int[2];

    // 시작 시 두 플레이어의 정보를 담고 있음
    public DummyPlayerParent[] playerList = new DummyPlayerParent[2];

    // 아이템 정보 모아둠
    public List<DummyItemData> itemDataList;

    // 사용할 아이템 모아둠
    public List<DummyItemParent> itemList;
    [Header("Value")]
    // 게임 상태
    public GameState gameState;

    // 안드로이드 전용
    private Touch LeftTouch;
    private Touch RightTouch;
    private int touchCount;

    private void Awake()
    {
        if (systemManager == null)
            systemManager = this;
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);

        Screen.SetResolution(1280, 720, true);
    }

    private void Update()
    {
        GestureManager();
    }

    public void LeftClick()
    {
        playerList[0].ChangeDirection();
    }

    public void RightClick()
    {
        playerList[1].ChangeDirection();
    }

    void GestureManager()
    {
        if (Input.touchCount > 0)
        {
            for(int i = 0; i < touchCount; i++)
            {
                if (LeftTouch.phase != TouchPhase.Ended && RightTouch.phase != TouchPhase.Ended)
                    break;

                if (Input.GetTouch(i).position.x < Screen.width / 2)
                {
                    LeftTouch = Input.GetTouch(i);
                }
                else
                {
                    RightTouch = Input.GetTouch(i);
                }            
            }

            Vector2 leftStartPos = LeftTouch.position;
            Vector2 leftEndPos = LeftTouch.position;
            Vector2 rightStartPos = RightTouch.position;
            Vector2 rightEndPos = RightTouch.position;

            switch (LeftTouch.phase)
            {
                case TouchPhase.Began:
                    leftStartPos = LeftTouch.position;
                    break;
                case TouchPhase.Moved:
                    break;
                case TouchPhase.Ended:
                    leftEndPos = LeftTouch.position;
                    Vector2 velocity = leftEndPos - leftStartPos;
                    if(velocity.sqrMagnitude > 2 && (Mathf.Atan2(velocity.x, velocity.y) * Mathf.Rad2Deg) > 60 && (Mathf.Atan2(velocity.x, velocity.y) * Mathf.Rad2Deg) < 120)
                    {
                        playerList[0].Jump();
                    }
                    else
                    {
                        playerList[0].ChangeDirection();
                    }
                    break;
            }

            switch (RightTouch.phase)
            {
                case TouchPhase.Began:
                    rightStartPos = RightTouch.position;
                    break;
                case TouchPhase.Moved:
                    break;
                case TouchPhase.Ended:
                    rightEndPos = RightTouch.position;
                    Vector2 velocity = rightEndPos - rightStartPos;
                    if (velocity.sqrMagnitude > 2 && (Mathf.Atan2(velocity.x, velocity.y) * Mathf.Rad2Deg) > 60 && (Mathf.Atan2(velocity.x, velocity.y) * Mathf.Rad2Deg) < 120)
                    {
                        playerList[1].Jump();
                    }
                    else
                    {
                        playerList[1].ChangeDirection();
                    }
                    break;
            }
        }
    }
}
