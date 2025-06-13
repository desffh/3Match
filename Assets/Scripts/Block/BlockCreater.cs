using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// 블럭 프리팹 생성, 오브젝트 풀 관리
/// </summary>

public class BlockCreater : MonoBehaviour
{
    [Header("블록 생성에 필요한 옵션")]
    [SerializeField] GameObject blockPrefab;  // 생성할 Prefab
    [SerializeField] Transform blockParent;   // 생성될 계층 구조 위치
    
    private ObjectPool<GameObject> blockPool; // 오브젝트 풀


    // 오브젝트 풀 초기화
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
    /// 오브젝트 풀에서 꺼내기
    /// </summary>
    /// <param name="position">   생성할 transform 위치             </param>
    /// <param name="data">       블럭 데이터                       </param>
    /// <param name="boardPos">   주입 할 Vector2Int 좌표값         </param>
    /// <param name="scale">      설정할 scale크기                  </param>
    /// <returns>생성될 블럭 오브젝트</returns>
    public Block SpawnBlock(Vector3 position, BlockData data, Vector2Int boardPos, Vector3 scale)
    {
        GameObject obj = blockPool.Get();
        obj.transform.position = position;         // 블럭 오브젝트 위치 설정
        obj.transform.localScale = scale;          // 블럭 오브젝트 크기 설정

        Block block = obj.GetComponent<Block>();
        block.SetBoardPos(boardPos.x, boardPos.y); // 좌표 주입 (행X,열Y)
        block.Init(data);                          // 랜덤 데이터 저장

        return block;
    }

    // 풀에 반납
    public void DespawnBlock(Block block)
    {
        block.gameObject.SetActive(false);
        blockPool.Release(block.gameObject);
    }
}
