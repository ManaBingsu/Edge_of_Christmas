using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummySystemManager : MonoBehaviour
{
    // 싱글톤
    public static DummySystemManager systemManager;

    // 현재 선택된 플레이어 스킨의 인덱스 정보, 로비 창에서 스킨을 선택했을 때 이 변수를 바꿔주면 된다
    public int[] playerSkinIndexArray = new int[2];

    // 시작 시 두 플레이어의 정보를 담고 있음
    public DummyPlayerParent[] playerList = new DummyPlayerParent[2];

    // 아이템 정보 모아둠
    public List<DummyItemData> itemDataList;

    // 사용할 아이템 모아둠
    public List<DummyItemParent> itemList;
    [Header("Value")]

    [Header("Manager Reference")]
    public DummyItemGenerator itemGenerator;

    private void Awake()
    {
        if (systemManager == null)
            systemManager = this;
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);

        Screen.SetResolution(1280, 720, true);
    }
}
