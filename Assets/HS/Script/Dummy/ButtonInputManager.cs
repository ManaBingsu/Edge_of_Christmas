using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInputManager : MonoBehaviour
{
    [SerializeField]
    private bool isSelected;

    [SerializeField]
    private bool isForceBreak;

    [SerializeField]
    private float forceMoveTime;

    private Coroutine actCoroutine;

    public void DragToAct()
    {
        if (!isSelected)
        {
            isSelected = true;
            if (actCoroutine != null)
            {
                actCoroutine = null;
            }
            actCoroutine = StartCoroutine(Activate());
        }

        //transform.position = new Vector3Input.mousePosition
    }

    IEnumerator Activate()
    {
        isSelected = true;
        float time = 0f;
        while((time += Time.deltaTime) < forceMoveTime)
        {
            if (isForceBreak)
                break;

            yield return null;
        }

    }

    public void DragOutAndMove()
    {

    }
}
