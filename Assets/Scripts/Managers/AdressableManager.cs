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

    // Addressable �ʱ�ȭ
    IEnumerator InitAddressable()
    {
        var init = Addressables.InitializeAsync();
        yield return init;
    }

    // ��ư Ŭ�� �� �ҷ�����
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

    // ��ư Ŭ�� �� �����ϱ�

    public void Button_Release()
    {
        // GameObject ����
        if (gameobjects.Count > 0)
        {
            var index = gameobjects.Count - 1;
            Addressables.ReleaseInstance(gameobjects[index]);
            gameobjects.RemoveAt(index);
        }

        // ScriptableObject ����
        if (loadedSOList.Count > 0)
        {
            var index = loadedSOList.Count - 1;
            Addressables.Release(loadedSOList[index]);
            loadedSOList.RemoveAt(index);
        }
    }
}
