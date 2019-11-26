using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudienceEmotion : MonoBehaviour
{
    public List<Sprite> emotionSprList;

    public Transform[] doorArray = new Transform[2];

    public GameObject emoticon;
    private SpriteRenderer sprRend;

    private void Awake()
    {
        sprRend = emoticon.GetComponent<SpriteRenderer>();
    }

    public void SetEmotion(int index)
    {
        sprRend.sprite = emotionSprList[index];
    }
}
