using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// ��� �̵� �ִϸ��̼� ó��
//
// ��︮�� ��Ʈ�� ã�Ƽ� ���߿� ����

public class BlockAnime : MonoBehaviour
{
    // ���콺 �Է� ���� �ִϸ��̼�
    public Tween MoveTo(Vector3 worldPos)
    {
        return transform.DOMove(worldPos, 0.2f).SetEase(Ease.OutQuart);
    }

    // ��ġ �� ���� �ִϸ��̼� 
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

    // ������ �� ���� �ִϸ��̼� 
    public void ResetScale(Vector3 blockScale)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(blockScale + new Vector3(0.1f, 0.1f, 0.1f), 0.2f).SetEase(Ease.OutSine));
        seq.Append(transform.DOScale(blockScale, 0.3f).SetEase(Ease.OutBack));

        seq.OnComplete(() =>
        {
            transform.localScale = blockScale; // Ǯ���� ���� �� ȣ���
        });
    }
}
