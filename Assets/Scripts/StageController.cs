using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class StageController : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private Stage stage;

    [Header("Addressable StageData 리스트")]
    [SerializeField] private AssetReferenceT<StageData>[] stageDataReferences;

    private AsyncOperationHandle<StageData>? currentHandle;

    public void LoadRandomStage()
    {
        if (stageDataReferences == null || stageDataReferences.Length == 0)
        {
            Debug.LogError("StageData 리스트가 비어 있습니다!");
            return;
        }

        int randomIndex = Random.Range(0, stageDataReferences.Length);
        var selectedRef = stageDataReferences[randomIndex];

        // 이전 로드 해제
        if (currentHandle.HasValue && currentHandle.Value.IsValid())
        {
            Addressables.Release(currentHandle.Value);
        }

        currentHandle = selectedRef.LoadAssetAsync();
        currentHandle.Value.Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                // 스테이지 호출 & 보드 세팅
                stage.Init(handle.Result);
                stage.SetupBoard();
                Debug.Log("스테이지 세팅 완료");
            }
            else
            {
                Debug.LogError("StageData 로드 실패");
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
