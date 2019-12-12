using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    // 싱글톤
    public static BattleManager battleManager;

    // 이긴 사람 인덱스
    public int winnerIndex;

    // 게임 상태 관리
    public enum GameState { Idle, BeforeStart, Processing, GameOver }
    // 게임 상태
    public GameState gameState;
    // 게임 상태 코루틴
    Coroutine gameCoroutine;

    // 게임 종료 시 패널
    [SerializeField]
    private UIGameOverPanel gameOverPanel;

    // 게임 시작 타이머
    [SerializeField]
    private Text TimerText;

    // Winner SpotLight
    [SerializeField]
    private GameObject spotLight;
    // Winner SpotLight
    [SerializeField]
    private GameObject stageLight;

    public UIGameOverPanel panel;

    [SerializeField]
    private ParticleSystem particle;

    public UIEnding uiEnding;


    // 옵션
    public UIOption uiOption;

    private void Awake()
    {
        if (battleManager == null)
            battleManager = this;
        else
            Destroy(this.gameObject);
    }

    private void Start()
    {
        StartCoroutine(StartTimer());
        StartCoroutine(FSM());
        int particleOn = PlayerPrefs.GetInt("IsParticle", 1);
        if (particleOn == 1)
            particle.Play();
        else
            particle.Stop();
    }

    private void Update()
    {
        if (Application.platform != RuntimePlatform.IPhonePlayer)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                uiOption.OnPanel();
            }
        }
    }

    IEnumerator StartTimer()
    {
        float time = 4f;
        while(time-- > 1f)
        {
            TimerText.text = time.ToString();
            yield return new WaitForSeconds(1.0f);
        }
        TimerText.gameObject.SetActive(false);
        gameState = GameState.Processing;
    }

    IEnumerator FSM()
    {
        while (true)
        {
            switch (gameState)
            {
                case GameState.Idle:
                    yield return gameCoroutine = StartCoroutine(Idle());
                    break;
                case GameState.BeforeStart:
                    yield return gameCoroutine = StartCoroutine(BeforeStart());
                    break;
                case GameState.Processing:
                    yield return gameCoroutine = StartCoroutine(Process());
                    break;
                case GameState.GameOver:
                    yield return gameCoroutine = StartCoroutine(GameOver());
                    break;
            }
        }
    }

    IEnumerator Idle()
    {
        while (gameState == GameState.Idle)
        {
            yield return null;
        }
    }

    IEnumerator BeforeStart()
    {
        BeforeStartInitSetting();

        while (gameState == GameState.BeforeStart)
        {
            yield return null;
        }
    }

    public void BeforeStartInitSetting()
    {
        DummySystemManager.systemManager.itemGenerator.state = DummyItemGenerator.State.Idle;
        DummySystemManager.systemManager.playerList[0].state = DummyPlayerParent.State.Idle;
        DummySystemManager.systemManager.playerList[1].state = DummyPlayerParent.State.Idle;

    }

    IEnumerator Process()
    {
        ProcessInitSetting();

        while (gameState == GameState.Processing)
        {
            yield return null;
        }

        ProcessEndSetting();
    }

    public void ProcessInitSetting()
    {
        DummySystemManager.systemManager.itemGenerator.state = DummyItemGenerator.State.Generating;
        DummySystemManager.systemManager.playerList[0].state = DummyPlayerParent.State.Walk;
        DummySystemManager.systemManager.playerList[1].state = DummyPlayerParent.State.Walk;
    }

    public void ProcessEndSetting()
    {
        DummySystemManager.systemManager.itemGenerator.state = DummyItemGenerator.State.Idle;
        DummySystemManager.systemManager.playerList[0].state = DummyPlayerParent.State.Idle;
        DummySystemManager.systemManager.playerList[1].state = DummyPlayerParent.State.Idle;
    }

    IEnumerator GameOver()
    {
        StartCoroutine(GameOverInitSetting());

        while (gameState == GameState.GameOver)
        {
            spotLight.transform.position = Vector3.Lerp(spotLight.transform.position, new Vector3
            (DummySystemManager.systemManager.playerList[winnerIndex].transform.position.x,
            DummySystemManager.systemManager.playerList[winnerIndex].transform.position.y + 0.5f,
            spotLight.transform.position.z), Time.deltaTime * 2f);
            yield return null;
        }
    }

    public IEnumerator GameOverInitSetting()
    {
        stageLight.SetActive(false);
        spotLight.SetActive(true);
        // 승리 카메라 흔들림 효과
        CameraControl.camControl.shakeCoroutine = StartCoroutine(CameraControl.camControl.Shake(6f, 1.5f));
        yield return new WaitForSeconds(3.5f);

        gameOverPanel.gameObject.SetActive(true);
        gameOverPanel.SetWinner(winnerIndex);
        gameOverPanel.OnGameOverPanel();

        uiEnding.SetWinImage();
    }


    // 게임오버 는 승리자 인덱스와 함께 이걸 실행할 것
    public void SetGameOver(int winIdx)
    {
        gameState = GameState.GameOver;
        winnerIndex = winIdx;
    }
}
