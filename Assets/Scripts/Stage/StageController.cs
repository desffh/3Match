using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

/// <summary>
/// �������� ���� 
/// </summary>

public class StageController : MonoBehaviour
{
    [SerializeField] private Stage stage;

    [SerializeField] private int stageNum;

    // ��巹���� ����
    [SerializeField] private List<StageData> stageData = new List<StageData>();

    private void Start()
    {
        LoadStage();
    }

    public void LoadStage()
    {
        stage.Init(stageData[stageNum]); // �������� ���� ����
        stage.SetupBoard(); // �������� ���� ����
    }
}
