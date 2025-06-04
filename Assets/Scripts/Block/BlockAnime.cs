using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// 블록 이동 애니메이션 처리
//
// 어울리는 두트윈 찾아서 나중에 수정

public class BlockAnime : MonoBehaviour
{
    // 마우스 입력 스왑 애니메이션
    public Tween MoveTo(Vector3 worldPos)
    {
        return transform.DOMove(worldPos, 0.2f).SetEase(Ease.OutQuart);
    }

    // 매치 시 머지 애니메이션 
    public void MergeToAndPop(Vector3 targetPos, Vector3 blockScale, System.Action onComplete = null)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOMove(targetPos, 0.25f).SetEase(Ease.InOutQuad));
        seq.Join(transform.DOScale(blockScale, 0.2f).SetEase(Ease.InBack));
        seq.OnComplete(() =>
        {
            gameObject.SetActive(false);
            onComplete?.Invoke();
        });
    }

    // 머지된 블럭 생성 애니메이션 
    public void ResetScale(Vector3 blockScale)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(blockScale + new Vector3(0.1f, 0.1f, 0.1f), 0.2f).SetEase(Ease.OutSine));
        seq.Append(transform.DOScale(blockScale, 0.3f).SetEase(Ease.OutBack));

        seq.OnComplete(() =>
        {
            transform.localScale = blockScale; // 풀에서 꺼낼 때 호출용
        });
    }
}
