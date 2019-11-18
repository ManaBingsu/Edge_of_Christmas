using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManagerDebug : MonoBehaviour
{
    public Text[] bug = new Text[10];
    public DummyInputManager dim;
    public int num = 0;

    public float dxx;
    public float rad;

    private void Start()
    {
        for(int i = 0; i < 10; i++)
        {
            bug[i] = transform.GetChild(i).GetComponent<Text>();
        }
    }
    
    private void Update()
    {
        for (int i = 0; i < 10; i++)
        {
            if (num == 0)
            {
                bug[i].text = dxx.ToString();
            }
                
            else
            {
                bug[i].text = rad.ToString();
            }

        }
    }
}
