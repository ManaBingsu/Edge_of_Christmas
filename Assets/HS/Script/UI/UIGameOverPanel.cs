using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIGameOverPanel : MonoBehaviour
{
    private int currentPanelIndex;

    [SerializeField]
    private List<GameObject> panelList;

    [SerializeField]
    private Transform[] placeList = new Transform[2];

    [SerializeField]
    private DummyPlayerGeneratorManager playerSkin;

    // Winner
    [SerializeField]
    private Image winnerImage;
    [SerializeField]
    private Text winnerText;

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

    public GameObject uiOption;

    public void OnGameOverPanel()
    {
        onOffCoroutine = StartCoroutine(OnOffPanel(true, 0));
    }

    public void ClickContinue()
    {
        Time.timeScale = 1f;
        uiOption.gameObject.SetActive(false);
    }


    public void ClickRetry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("HSScene");
    }

    public void ClickLobby()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("HSLobbyScene");
    }

    public void SetWinner(int index)
    {
        int skinIndex = DummySystemManager.systemManager.playerSkinIndexArray[index];
        DummyPlayerSkinData data = playerSkin.skinDataList[skinIndex];

        winnerImage.sprite = data.Icon;
        string winnerTeam = index == 0 ? "왼쪽" : "오른쪽";
        winnerText.text = winnerTeam + "\r\n'" + data.SkinName + "'\r\n승리!";
    }

    public IEnumerator OnOffPanel(bool isOn, int index)
    {
        if (isOn)
        {
            touchLimitImage.gameObject.SetActive(true);
            panelList[index].SetActive(true);
        }


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

        if (!isOn)
        {
            touchLimitImage.gameObject.SetActive(false);
            panelList[index].SetActive(false);
        }
    }
}
