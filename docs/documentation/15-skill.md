# Skill 만들기

1. Skill을 상속하고 Execute에 효과를 작성합니다.
2. 관리자 소유자 설정 후 AddSkill로 등록합니다.
3. 입력·게임 로직에서 Execute를 호출합니다.
4. 시간 갱신이 필요할 때만 UpdateTick을 확장합니다.

위치: Assets/Scripts/InGame/Skill. 등록과 갱신은 자동 실행이 아닙니다.

```csharp
/// <summary>
/// 스킬 확장용 뼈대를 제공한다.
/// </summary>
public class ExampleSkill : Skill
{
    /// <summary>
    /// 소유자가 연결된 경우에만 효과를 실행한다.
    /// </summary>
    public override void Execute()
    {
        if (owner == null) return;
        // 실제 효과를 작성한다.
    }
}
```

쿨다운·비용·대상 선택은 기본 구현에 없습니다. 뼈대의 컴파일·실행은 미확인입니다.
