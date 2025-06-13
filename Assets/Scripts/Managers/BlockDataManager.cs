using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �� ������ SO�� ��ųʸ��� ��� �����ϰ� �ִ� BlockDataManager
/// ���� ���� ���� Num���� �����Ͽ� ������ �ҷ����� ����
/// </summary>

[CreateAssetMenu(menuName = "Block/BlockDataManager")]

public class BlockDataManager : ScriptableObject
{
    public List<BlockData> blockDataList;
    
    // �� ���� ���ڸ� ���� ������ ���� ����
    private Dictionary<int, BlockData> blockDataDict;


    /// <summary>
    /// �ʱ�ȭ (�ߺ����� Dictionary�� 1~6�� �ش��ϴ� BlockData �Ҵ�)
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
                Debug.Log($"�ߺ��� Num {data.Num}");
            }
        }
    }


    /// <summary>
    /// num�� �ش��ϴ� BlockData�� ã�� ��ȯ
    /// </summary>
    /// <param name="num">  KEY��  </param>
    /// <returns>          VALUE�� </returns>
    public BlockData Get(int num)
    {
        if (blockDataDict == null)
        {
            return null;
        }

        // KEY���� ���� BlockData ��ȯ 
        if (blockDataDict.TryGetValue(num, out BlockData data))
        {
            return data;
        }

        Debug.LogWarning($"{num}�� �ش��ϴ� �� �����Ͱ� ����");
        return null;
    }
}
