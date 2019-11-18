using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStory : MonoBehaviour
{
    [SerializeField]
    private int key;

    [SerializeField]
    private GameObject[] buttonList = new GameObject[2];

    [SerializeField]
    private List<Story> storyList;

    public void ClickNext()
    {
        if(key + 1 > storyList.Count - 1)
        {
            buttonList[1].SetActive(false);
            key = storyList.Count - 1;
        }
        else
        {
            key++;
        }
    }

    public void ClickPrevious()
    {

    }
}

public class Story
{
    public Sprite illust;
    public string script;
}