using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelManager : MonoBehaviour
{
    private int currentPanelIndex;

    [SerializeField]
    private GameObject[] panelList = new GameObject[3];

    [SerializeField]
    private Transform[] placeList = new Transform[2];

    [SerializeField]
    private Image touchLimitImage;

    [SerializeField]
    private float speed;

    [SerializeField]
    private Color limitColor;
    [SerializeField]
    private Color clearColor;

    [SerializeField]
    private AnimationCurve moveCurve;

    private Coroutine onOffCoroutine;

    public void OnGameInfoPanel()
    {
        if (onOffCoroutine != null)
            StopCoroutine(onOffCoroutine);

        onOffCoroutine = StartCoroutine(OnOffPanel(0, true));
    }
    public void OnStoryPanel()
    {
        if (onOffCoroutine != null)
            StopCoroutine(onOffCoroutine);

        onOffCoroutine = StartCoroutine(OnOffPanel(1, true));
    }
    public void OnPlayerSkinPanel()
    {
        if (onOffCoroutine != null)
            StopCoroutine(onOffCoroutine);

        onOffCoroutine = StartCoroutine(OnOffPanel(2, true));
    }

    public void OffCurrentPanel()
    {
        if (onOffCoroutine != null)
            StopCoroutine(onOffCoroutine);

        onOffCoroutine = StartCoroutine(OnOffPanel(currentPanelIndex, false));
    }

    public IEnumerator OnOffPanel(int index, bool isOn)
    {
        if(isOn)
        {
            touchLimitImage.gameObject.SetActive(true);
            panelList[index].SetActive(true);
        }

        currentPanelIndex = index;

        Color currentColor = touchLimitImage.color;
        Vector2 currentPos = panelList[index].transform.position;

        Color targetColor;
        Vector2 endPos;

        if (isOn)
        {
            targetColor = limitColor;
            endPos = placeList[1].position;
        }
        else
        {
            targetColor = clearColor;
            endPos = placeList[0].position;
        }

        float progress = 0f;
        while (progress < 1.0f)
        {
            progress += Time.deltaTime * speed;

            touchLimitImage.color = Color.Lerp(currentColor, targetColor, progress);
            panelList[index].transform.position = Vector2.Lerp(currentPos, endPos, moveCurve.Evaluate(progress));

            yield return null;
        }

        if(!isOn)
        {
            touchLimitImage.gameObject.SetActive(false);
            panelList[index].SetActive(false);
        }
    }
}

