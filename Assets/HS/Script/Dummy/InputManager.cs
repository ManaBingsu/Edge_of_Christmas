using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public List<touchLocation> touches = new List<touchLocation>();

    private void Update()
    {
        int i = 0;
        while (i < Input.touchCount)
        {
            if (EventSystem.current.IsPointerOverGameObject(i) == true)
                return;

            Touch t = Input.GetTouch(i);
            switch(t.phase)
            {
                case TouchPhase.Began:
                    int index = t.position.x < Screen.width / 2 ? 0 : 1;
                    touches.Add(new touchLocation(t.fingerId, index));
                    touchLocation startTouch = touches.Find(touchLocation => touchLocation.touchId == t.fingerId);
                    startTouch.startPos = t.position;
                    break;
                case TouchPhase.Ended:
                    touchLocation endedTouch = touches.Find(touchLocation => touchLocation.touchId == t.fingerId);
                    if(!endedTouch.isAct)
                    {
                        Vector3 dddir = (endedTouch.endPos - endedTouch.startPos).normalized;
                        float rrad = Quaternion.FromToRotation(Vector3.up, dddir).eulerAngles.z;
                        float ddistance = Vector2.Distance(endedTouch.startPos, endedTouch.endPos);

                        if (rrad > 320 || rrad < 40)
                        {
                            if (ddistance > 60)
                            {
                                DummySystemManager.systemManager.playerList[endedTouch.playerIndex].Jump();
                            }
                            else
                            {
                                DummySystemManager.systemManager.playerList[endedTouch.playerIndex].ChangeDirection();
                            }
                        }
                        else
                        {
                            DummySystemManager.systemManager.playerList[endedTouch.playerIndex].ChangeDirection();
                        }
                    }
                    touches.RemoveAt(touches.IndexOf(endedTouch));
                    break;
                case TouchPhase.Moved:
                    touchLocation movedTouch = touches.Find(touchLocation => touchLocation.touchId == t.fingerId);
                    movedTouch.endPos = t.position;
                    movedTouch.timer += Time.deltaTime;

                    Vector3 ddir = (movedTouch.endPos - movedTouch.startPos).normalized;
                    float rad = Quaternion.FromToRotation(Vector3.up, ddir).eulerAngles.z;
                    float distance = Vector2.Distance(movedTouch.startPos, movedTouch.endPos);

                    if(movedTouch.timer > 0.03f && movedTouch.isAct == false)
                    {
                        movedTouch.isAct = true;
                        if (rad > 315 || rad < 45)
                        {
                            if (distance > 50)
                            {
                                DummySystemManager.systemManager.playerList[movedTouch.playerIndex].Jump();
                            }
                            else
                            {
                                DummySystemManager.systemManager.playerList[movedTouch.playerIndex].ChangeDirection();
                            }
                        }
                        else
                        {
                            DummySystemManager.systemManager.playerList[movedTouch.playerIndex].ChangeDirection();
                        }
                    }

                    break;
            }
            ++i;
        }
    }
}

public class touchLocation
{
    public int touchId;
    public int playerIndex;
    public bool isAct;
    public float timer;

    public Vector2 startPos;
    public Vector2 endPos;

    public touchLocation(int id, int index)
    {
        this.touchId = id;
        this.playerIndex = index;
    }
}
