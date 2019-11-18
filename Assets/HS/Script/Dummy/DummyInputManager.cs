using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; 

public class DummyInputManager : MonoBehaviour
{
    delegate void listener(string type, int id, float x, float y, float dx, float dy, Vector2 dir);

    event listener begin0, begin1, begin2, begin3, begin4;
    event listener move0, move1, move2, move3, move4;
    event listener end0, end1, end2, end3, end4;

    private Vector2[] delta = new Vector2[5];

    private float[] timer = new float[5];
    private int[] playerIndex = new int[5];
    private bool[] isDone = new bool[5];

    public float touchSensitive;

    private void FixedUpdate()
    {
            ManageTouch();
    }

    void ManageTouch()
    {
        int count = Input.touchCount;
        if (count == 0 || count > 5) return;

        for (int i = 0; i < count; i++)
        {
            Touch touch = Input.GetTouch(i);
            int id = touch.fingerId;

            //터치좌표
            Vector2 pos = touch.position;

            //begin이라면 무조건 delta에 넣어주자.
            if (touch.phase == TouchPhase.Began) delta[id] = touch.position;

            //좌표계 정리
            float x, y, dx, dy;
            Vector2 dir;
            x = pos.x;
            y = pos.y;
            if (touch.phase == TouchPhase.Began)
            {
                dx = dy = 0;
                dir = new Vector2(0, 0);
            }
            else
            {
                dx = pos.x - delta[id].x;
                dy = pos.y - delta[id].y;
                dir = (pos - delta[id]).normalized;
            }

            //상태에 따라 이벤트를 호출하자
            if (touch.phase == TouchPhase.Began)
            {
                switch (id)
                {
                    case 0: if (begin0 != null) begin0("begin", id, x, y, dx, dy, dir); break;
                    case 1: if (begin1 != null) begin1("begin", id, x, y, dx, dy, dir); break;
                    case 2: if (begin2 != null) begin2("begin", id, x, y, dx, dy, dir); break;
                    case 3: if (begin3 != null) begin3("begin", id, x, y, dx, dy, dir); break;
                    case 4: if (begin4 != null) begin4("begin", id, x, y, dx, dy, dir); break;
                }
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                switch (id)
                {
                    case 0: if (move0 != null) move0("move", id, x, y, dx, dy, dir); break;
                    case 1: if (move1 != null) move1("move", id, x, y, dx, dy, dir); break;
                    case 2: if (move2 != null) move2("move", id, x, y, dx, dy, dir); break;
                    case 3: if (move3 != null) move3("move", id, x, y, dx, dy, dir); break;
                    case 4: if (move4 != null) move4("move", id, x, y, dx, dy, dir); break;
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                switch (id)
                {
                    case 0: if (end0 != null) end0("end", id, x, y, dx, dy, dir); break;
                    case 1: if (end1 != null) end1("end", id, x, y, dx, dy, dir); break;
                    case 2: if (end2 != null) end2("end", id, x, y, dx, dy, dir); break;
                    case 3: if (end3 != null) end3("end", id, x, y, dx, dy, dir); break;
                    case 4: if (end4 != null) end4("end", id, x, y, dx, dy, dir); break;
                }
            }
        }
    }

    void Start()
    {
        begin0 += onTouch;
        end0 += onTouch;
        move0 += onTouch;

        begin1 += onTouch;
        end1 += onTouch;
        move1 += onTouch;

        begin2 += onTouch;
        end2 += onTouch;
        move2 += onTouch;

        begin3 += onTouch;
        end3 += onTouch;
        move3 += onTouch;

        begin4 += onTouch;
        end4 += onTouch;
        move4 += onTouch;
    }

    void onTouch(string type, int id, float x, float y, float dx, float dy, Vector2 dir)
    {
        if (EventSystem.current.IsPointerOverGameObject(id) == true)
            return;

        switch (type)
        {
            case "begin":
                timer[id] = 0;
                playerIndex[id] = x < Screen.width / 2 ? 0 : 1;
                isDone[id] = false;
                break;
            case "move":

                Vector3 ddir = new Vector3(dir.x, dir.y, 0);
                float rad = Quaternion.FromToRotation(Vector3.up, ddir).eulerAngles.z;
                float distance = dx * dx + dy * dy;

                timer[id] += Time.deltaTime;
                if(timer[id] > 0.03f && !isDone[id])
                {
                    isDone[id] = true;
                    if (rad > 320 || rad < 40)
                    {
                        if (distance > 300)
                        {
                            DummySystemManager.systemManager.playerList[playerIndex[id]].Jump();
                        }
                        else
                        {
                            DummySystemManager.systemManager.playerList[playerIndex[id]].ChangeDirection();
                        }
                    }
                    else
                    {
                        DummySystemManager.systemManager.playerList[playerIndex[id]].ChangeDirection();
                    }                  
                }
       
                break;
            case "end":

                if (!isDone[id])
                {
                    Vector3 dddir = new Vector3(dir.x, dir.y, 0);
                    float rrad = Quaternion.FromToRotation(Vector3.up, dddir).eulerAngles.z;
                    float ddistance = dx * dx + dy * dy;

                    if (rrad > 320 || rrad < 40)
                    {
                        if (ddistance > 300)
                        {
                            DummySystemManager.systemManager.playerList[playerIndex[id]].Jump();
                        }
                        else
                        {
                            DummySystemManager.systemManager.playerList[playerIndex[id]].ChangeDirection();
                        }
                    }
                    else
                    {
                        DummySystemManager.systemManager.playerList[playerIndex[id]].ChangeDirection();
                    }
                }
                timer[id] = 0;
                isDone[id] = false;
                break;
        }
    }
}