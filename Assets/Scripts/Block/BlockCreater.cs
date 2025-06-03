using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

// 블럭 생성용 스크립트

public class BlockCreater : MonoBehaviour
{
    [SerializeField] GameObject blockPrefab;

    [SerializeField] Transform blockParent;

    // 오브젝트 풀
    private ObjectPool<GameObject> blockPool;

    // 초기화
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

    // 풀에서 꺼내기
    public Block SpawnBlock(Vector3 position, BlockData data, Vector2Int boardPos, Vector3 scale)
    {
        GameObject obj = blockPool.Get();
        obj.transform.position = position;
        obj.transform.localScale = scale;

        Block block = obj.GetComponent<Block>();
        block.SetBoardPos(boardPos.x, boardPos.y); // 좌표 주입
        block.Init(data); // 랜덤 데이터 저장

        return block;
    }

    // 풀에 반납
    public void DespawnBlock(Block block)
    {
        block.gameObject.SetActive(false);
        blockPool.Release(block.gameObject);
    }
}
