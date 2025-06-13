using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// 블록 이동 애니메이션 처리
/// </summary>

public class BlockAnime : MonoBehaviour
{
    /// <summary>
    /// 블럭 애니메이션 (Swap & Drop)
    /// </summary>
    /// <param name="worldPos">   이동할 위치 (Vector3 transform 좌표값)</param>
    /// <param name="ease"    >   적용할 DoTween 이징 함수</param>
    /// <param name="duration">   애니메이션 지속 시간 (초)</param>
    /// <returns>트윈(Tween) 객체 반환 - 트윈 완료 여부를 판별하기 위함</returns>
    public Tween MoveTo(Vector3 worldPos, Ease ease, float duration = 0.2f)
    {
        return transform.DOMove(worldPos, duration).SetEase(ease);
    }



    /// <summary>
    /// 매치 시 병합 애니메이션 
    /// </summary>
    /// <param name="targetPos" >    이동할 위치 (Vector3 transform 좌표값)</param>
    /// <param name="blockScale">    변경할 크기 (Vector3 scale)</param>
    /// <param name="onComplete">    애니메이션이 모두 완료되었을 시 호출 할 이벤트</param>
    public void MergeToBlockAnime(Vector3 targetPos, Vector3 blockScale, System.Action onComplete = null)
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOMove(targetPos, 0.15f).SetEase(Ease.InOutQuad));
        seq.Join(transform.DOScale(blockScale, 0.2f).SetEase(Ease.InBack));
        seq.OnComplete(() =>
        {
            gameObject.SetActive(false);
            onComplete?.Invoke();
        });
    }



    /// <summary>
    /// 병합된 블럭 생성 애니메이션 
    /// </summary>
    /// <param name="blockScale">   변경할 크기 (Vector3 scale)</param>
    public void CreateBlockAnime(Vector3 blockScale)
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOScale(blockScale + new Vector3(0.1f, 0.1f, 0.1f), 0.2f).SetEase(Ease.OutSine));
        seq.Append(transform.DOScale(blockScale, 0.2f).SetEase(Ease.OutBack));

        seq.OnComplete(() =>
        {
            transform.localScale = blockScale; // 풀에서 꺼낼 때 호출용
        });
    }

}
