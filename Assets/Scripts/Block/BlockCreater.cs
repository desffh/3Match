using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// �� ������ ����, ������Ʈ Ǯ ����
/// </summary>

public class BlockCreater : MonoBehaviour
{
    [Header("��� ������ �ʿ��� �ɼ�")]
    [SerializeField] GameObject blockPrefab;  // ������ Prefab
    [SerializeField] Transform blockParent;   // ������ ���� ���� ��ġ
    
    private ObjectPool<GameObject> blockPool; // ������Ʈ Ǯ


    // ������Ʈ Ǯ �ʱ�ȭ
    public void Init()
    {
        blockPool = new ObjectPool<GameObject>
        (
            createFunc: () => Instantiate(blockPrefab, blockParent),
            actionOnGet: obj => obj.SetActive(true),
            actionOnRelease: obj => obj.SetActive(false),
            defaultCapacity: 64
        );
    }

    /// <summary>
    /// ������Ʈ Ǯ���� ������
    /// </summary>
    /// <param name="position">   ������ transform ��ġ             </param>
    /// <param name="data">       �� ������                       </param>
    /// <param name="boardPos">   ���� �� Vector2Int ��ǥ��         </param>
    /// <param name="scale">      ������ scaleũ��                  </param>
    /// <returns>������ �� ������Ʈ</returns>
    public Block SpawnBlock(Vector3 position, BlockData data, Vector2Int boardPos, Vector3 scale)
    {
        GameObject obj = blockPool.Get();
        obj.transform.position = position;         // �� ������Ʈ ��ġ ����
        obj.transform.localScale = scale;          // �� ������Ʈ ũ�� ����

        Block block = obj.GetComponent<Block>();
        block.SetBoardPos(boardPos.x, boardPos.y); // ��ǥ ���� (��X,��Y)
        block.Init(data);                          // ���� ������ ����

        return block;
    }

    // Ǯ�� �ݳ�
    public void DespawnBlock(Block block)
    {
        block.gameObject.SetActive(false);
        blockPool.Release(block.gameObject);
    }
}
