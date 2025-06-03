using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

// �� ������ ��ũ��Ʈ

public class BlockCreater : MonoBehaviour
{
    [SerializeField] GameObject blockPrefab;

    [SerializeField] Transform blockParent;

    // ������Ʈ Ǯ
    private ObjectPool<GameObject> blockPool;

    // �ʱ�ȭ
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

    // Ǯ���� ������
    public Block SpawnBlock(Vector3 position, BlockData data, Vector2Int boardPos, Vector3 scale)
    {
        GameObject obj = blockPool.Get();
        obj.transform.position = position;
        obj.transform.localScale = scale;

        Block block = obj.GetComponent<Block>();
        block.SetBoardPos(boardPos.x, boardPos.y); // ��ǥ ����
        block.Init(data); // ���� ������ ����

        return block;
    }

    // Ǯ�� �ݳ�
    public void DespawnBlock(Block block)
    {
        block.gameObject.SetActive(false);
        blockPool.Release(block.gameObject);
    }
}
