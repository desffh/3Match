using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

/// <summary>
/// 스테이지 선택 
/// </summary>

public class StageController : MonoBehaviour
{
    [SerializeField] private Stage stage;

    [SerializeField] private int stageNum;

    // 어드레서블 변경
    [SerializeField] private List<StageData> stageData = new List<StageData>();

    private void Start()
    {
        LoadStage();
    }

    public void LoadStage()
    {
        stage.Init(stageData[stageNum]); // 스테이지 정보 지정
        stage.SetupBoard(); // 스테이지 보드 구성
    }
}
