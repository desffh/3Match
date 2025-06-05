using System.Collections.Generic;

/// <summary>
/// 개별 블럭 매치 판별
/// </summary>

public interface IMatchRule
{
    // 개별 매치 로직
    bool isMatch(List<Block> group);

    // 매치된 블럭 반환
    List<Block> ExtractMatchBlocks(List <Block> group);

    // 병합을 위한 매치 타입
    MatchType matchType { get; }

    // 우선 순위
    int priority { get; } 
}

