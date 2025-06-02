using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AdressableManager : MonoBehaviour
{
    [SerializeField]
    private AssetReferenceGameObject blockObj;

    [SerializeField]
    private AssetReferenceGameObject cellObj;

    [SerializeField]
    private AssetReferenceT<BlockData> []blockdata;

    [SerializeField]
    private AssetReferenceT<CellData> celldata;

    private List<GameObject> gameobjects = new List<GameObject>();

    private List<ScriptableObject> loadedSOList = new List<ScriptableObject>();


    void Start()
    {
        StartCoroutine(InitAddressable());
    }

    // Addressable 초기화
    IEnumerator InitAddressable()
    {
        var init = Addressables.InitializeAsync();
        yield return init;
    }

    // 버튼 클릭 시 불러오기
    public void Button_SpawnObject()
    {
        // GameObject
        blockObj.InstantiateAsync().Completed += (obj) =>
        {
            gameobjects.Add(obj.Result);
        };
        cellObj.InstantiateAsync().Completed += (obj) =>
        {
            gameobjects.Add(obj.Result);
        };

        // ScriptableObject
        for(int i = 0; i < blockdata.Length; i++)
        {
            blockdata[i].LoadAssetAsync().Completed += (obj) =>
            {
                loadedSOList.Add(obj.Result);
            };
        }
        celldata.LoadAssetAsync().Completed += (obj) =>
        {
            loadedSOList.Add(obj.Result);
        };
    }

    // 버튼 클릭 시 해제하기

    public void Button_Release()
    {
        // GameObject 해제
        if (gameobjects.Count > 0)
        {
            var index = gameobjects.Count - 1;
            Addressables.ReleaseInstance(gameobjects[index]);
            gameobjects.RemoveAt(index);
        }

        // ScriptableObject 해제
        if (loadedSOList.Count > 0)
        {
            var index = loadedSOList.Count - 1;
            Addressables.Release(loadedSOList[index]);
            loadedSOList.RemoveAt(index);
        }
    }
}
