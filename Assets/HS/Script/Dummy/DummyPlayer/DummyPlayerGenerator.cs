using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyPlayerGenerator : MonoBehaviour
{
    public List<GameObject> playerSkinList;

    [SerializeField]
    public Transform[] generateTransform = new Transform[2];

    public void GeneratePlayer(int index)
    {
        // 플레이어 스킨 인덱스에 따라 소환
        GameObject obj = Instantiate(
            playerSkinList[DummySystemManager.systemManager.playerSkinIndexArray[index]],
            generateTransform[index].position, Quaternion.identity);

        // 소환한 플레이어를 시스템 매니저에 등록
        DummySystemManager.systemManager.playerList[index] = obj.GetComponent<DummyPlayerParent>();
        // 팀 방향 부여 ex) 왼쪽팀인지 오른쪽 팀인지
        int idx = index > 0 ? 1 : -1;
        obj.GetComponent<DummyPlayerParent>().playerData.team = (DummyPlayerData.Team)(idx);
        obj.SetActive(true);
    }
}
