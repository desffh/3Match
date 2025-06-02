using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class StageController : MonoBehaviour
{
    [Header("����")]
    [SerializeField] private Stage stage;

    [Header("Addressable StageData ����Ʈ")]
    [SerializeField] private AssetReferenceT<StageData>[] stageDataReferences;

    private AsyncOperationHandle<StageData>? currentHandle;

    public void LoadRandomStage()
    {
        if (stageDataReferences == null || stageDataReferences.Length == 0)
        {
            Debug.LogError("StageData ����Ʈ�� ��� �ֽ��ϴ�!");
            return;
        }

        int randomIndex = Random.Range(0, stageDataReferences.Length);
        var selectedRef = stageDataReferences[randomIndex];

        // ���� �ε� ����
        if (currentHandle.HasValue && currentHandle.Value.IsValid())
        {
            Addressables.Release(currentHandle.Value);
        }

        currentHandle = selectedRef.LoadAssetAsync();
        currentHandle.Value.Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                // �������� ȣ�� & ���� ����
                stage.Init(handle.Result);
                stage.SetupBoard();
                Debug.Log("�������� ���� �Ϸ�");
            }
            else
            {
                Debug.LogError("StageData �ε� ����");
            }
        };
    }

    private void OnDestroy()
    {
        if (currentHandle.HasValue && currentHandle.Value.IsValid())
        {
            Addressables.Release(currentHandle.Value);
        }
    }

    public void OnRandomStageButtonClick()
    {
        LoadRandomStage();
    }

}
