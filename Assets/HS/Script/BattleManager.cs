using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    private void Awake()
    {
        if (battleManager == null)
            battleManager = this;
        else
            Destroy(this.gameObject);
    }

    private void Start()
    {
        StartCoroutine(FSM());
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
        GameOverInitSetting();

        while (gameState == GameState.GameOver)
        {
            yield return null;
        }
    }

    public void GameOverInitSetting()
    {
        gameOverPanel.gameObject.SetActive(true);
        gameOverPanel.SetWinner(winnerIndex);
        gameOverPanel.OnGameOverPanel();
    }

    // 게임오버 는 승리자 인덱스와 함께 이걸 실행할 것
    public void SetGameOver(int winIdx)
    {
        gameState = GameState.GameOver;
        winnerIndex = winIdx;
    }
}
