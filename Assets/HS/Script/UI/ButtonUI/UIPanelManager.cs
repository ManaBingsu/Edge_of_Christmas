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

    private int backgroundIndex;
    private Coroutine eggCoroutine;
    public Image backGroundImage;
    public Sprite[] backGrouondImages = new Sprite[2];

    // 옵션
    public UIOption uiOption;

    private void Awake()
    {
        
    }

    private void Start()
    {
        uiOption.SaveParticle = PlayerPrefs.GetInt("IsParticle", 1);
    }

    private void Update()
    {
        if (Application.platform != RuntimePlatform.IPhonePlayer)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                OffCurrentPanel();
            }
        }
    }

    public void OnGameInfoPanel()
    {
        if (onOffCoroutine != null)
            StopCoroutine(onOffCoroutine);

        onOffCoroutine = StartCoroutine(OnOffPanel(0, true));
        SoundManager.sm.EfcClickButton();
    }
    public void OnStoryPanel()
    {
        if (onOffCoroutine != null)
            StopCoroutine(onOffCoroutine);

        onOffCoroutine = StartCoroutine(OnOffPanel(1, true));
        SoundManager.sm.EfcClickButton();
    }
    public void OnPlayerSkinPanel()
    {
        if (onOffCoroutine != null)
            StopCoroutine(onOffCoroutine);

        onOffCoroutine = StartCoroutine(OnOffPanel(2, true));
        SoundManager.sm.EfcClickButton();
    }

    public void OnOptionPanel()
    {
        if (onOffCoroutine != null)
            StopCoroutine(onOffCoroutine);

        onOffCoroutine = StartCoroutine(OnOffPanel(3, true));
        SoundManager.sm.EfcClickButton();
    }

    public void OnCreditPanel()
    {
        if (onOffCoroutine != null)
            StopCoroutine(onOffCoroutine);

        onOffCoroutine = StartCoroutine(OnOffPanel(4, true));
        SoundManager.sm.EfcClickButton();
    }

    public void DownLogoSpecialBackground()
    {
        if (eggCoroutine != null)
            StopCoroutine(eggCoroutine);
        eggCoroutine = StartCoroutine(Timer());
    }

    public void UpLogoSpecialBackground()
    {
        if (eggCoroutine != null)
            StopCoroutine(eggCoroutine);
    }

    IEnumerator Timer()
    {
        int time = 0;
        while (time <= 2.0f)
        {
            time++;
            yield return new WaitForSeconds(1.0f);
        }
        backgroundIndex = backgroundIndex == 0 ? 1 : 0;
        backGroundImage.sprite = backGrouondImages[backgroundIndex];
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

