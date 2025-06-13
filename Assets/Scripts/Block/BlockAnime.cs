using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// ��� �̵� �ִϸ��̼� ó��
/// </summary>

public class BlockAnime : MonoBehaviour
{
    /// <summary>
    /// �� �ִϸ��̼� (Swap & Drop)
    /// </summary>
    /// <param name="worldPos">   �̵��� ��ġ (Vector3 transform ��ǥ��)</param>
    /// <param name="ease"    >   ������ DoTween ��¡ �Լ�</param>
    /// <param name="duration">   �ִϸ��̼� ���� �ð� (��)</param>
    /// <returns>Ʈ��(Tween) ��ü ��ȯ - Ʈ�� �Ϸ� ���θ� �Ǻ��ϱ� ����</returns>
    public Tween MoveTo(Vector3 worldPos, Ease ease, float duration = 0.2f)
    {
        return transform.DOMove(worldPos, duration).SetEase(ease);
    }



    /// <summary>
    /// ��ġ �� ���� �ִϸ��̼� 
    /// </summary>
    /// <param name="targetPos" >    �̵��� ��ġ (Vector3 transform ��ǥ��)</param>
    /// <param name="blockScale">    ������ ũ�� (Vector3 scale)</param>
    /// <param name="onComplete">    �ִϸ��̼��� ��� �Ϸ�Ǿ��� �� ȣ�� �� �̺�Ʈ</param>
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
    /// ���յ� �� ���� �ִϸ��̼� 
    /// </summary>
    /// <param name="blockScale">   ������ ũ�� (Vector3 scale)</param>
    public void CreateBlockAnime(Vector3 blockScale)
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOScale(blockScale + new Vector3(0.1f, 0.1f, 0.1f), 0.2f).SetEase(Ease.OutSine));
        seq.Append(transform.DOScale(blockScale, 0.2f).SetEase(Ease.OutBack));

        seq.OnComplete(() =>
        {
            transform.localScale = blockScale; // Ǯ���� ���� �� ȣ���
        });
    }

}
