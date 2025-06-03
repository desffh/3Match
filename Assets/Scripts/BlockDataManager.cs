using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Block/BlockDataManager")]
public class BlockDataManager : ScriptableObject
{
    [SerializeField] private List<BlockData> blockDataList;

    private Dictionary<int, BlockData> blockDataDict;

    public void Init()
    {
        blockDataDict = new Dictionary<int, BlockData>();

        foreach (var data in blockDataList)
        {
            if (!blockDataDict.ContainsKey(data.Num))
                blockDataDict[data.Num] = data;
            else
                Debug.LogWarning($"Duplicate Num {data.Num} in BlockDataManager.");
        }
    }

    public BlockData Get(int num)
    {
        if (blockDataDict == null)
        {
            Debug.LogError("BlockDataManager not initialized!");
            return null;
        }

        if (blockDataDict.TryGetValue(num, out var data))
            return data;

        Debug.LogWarning($"BlockData Num {num} not found.");
        return null;
    }
}
