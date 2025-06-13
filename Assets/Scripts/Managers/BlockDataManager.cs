using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 블럭 데이터 SO를 딕셔너리에 모두 저장하고 있는 BlockDataManager
/// 블럭의 고유 숫자 Num으로 접근하여 데이터 불러오기 가능
/// </summary>

[CreateAssetMenu(menuName = "Block/BlockDataManager")]

public class BlockDataManager : ScriptableObject
{
    public List<BlockData> blockDataList;
    
    // 블럭 고유 숫자를 통해 데이터 접근 가능
    private Dictionary<int, BlockData> blockDataDict;


    /// <summary>
    /// 초기화 (중복없이 Dictionary에 1~6에 해당하는 BlockData 할당)
    /// </summary>
    public void Init()
    {
        blockDataDict = new Dictionary<int, BlockData>();

        foreach (BlockData data in blockDataList)
        {
            if (!blockDataDict.ContainsKey(data.Num))
            {
                blockDataDict[data.Num] = data;
            }
            else
            {
                Debug.Log($"중복된 Num {data.Num}");
            }
        }
    }


    /// <summary>
    /// num에 해당하는 BlockData를 찾고 반환
    /// </summary>
    /// <param name="num">  KEY값  </param>
    /// <returns>          VALUE값 </returns>
    public BlockData Get(int num)
    {
        if (blockDataDict == null)
        {
            return null;
        }

        // KEY값을 통해 BlockData 반환 
        if (blockDataDict.TryGetValue(num, out BlockData data))
        {
            return data;
        }

        Debug.LogWarning($"{num}에 해당하는 블럭 데이터가 없음");
        return null;
    }
}
