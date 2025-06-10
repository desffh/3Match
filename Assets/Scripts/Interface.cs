using System.Collections.Generic;

/// <summary>
/// ���� �� ��ġ �Ǻ�
/// </summary>

public interface IMatchRule
{
    // ��ġ�� �� ��ȯ
    List<Block> ExtractMatchBlocks(List <Block> group);

    // ������ ���� ��ġ Ÿ��
    MatchType matchType { get; }

    // �켱 ����
    int priority { get; } 
}

