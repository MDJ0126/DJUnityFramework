# Enum 작성

- 정해진 종류·상태를 나타낼 때 사용한다.
- 타입은 `e + PascalCase`, 항목은 `PascalCase`로 작성한다.
- 기본값은 `None = 0`으로 둔다. 레이어처럼 외부 값에 대응하면 해당 숫자를 따른다.
- 저장·직렬화되는 값은 숫자를 명시하고, 기존 숫자를 바꾸거나 재사용하지 않는다.

## 선언·사용 예제

프로젝트의 `eBuffType`을 기준으로 주석을 붙인 예제다.

```csharp
/// <summary>
/// 버프 종류를 구분한다.
/// </summary>
public enum eBuffType
{
    None = 0,
    Heal = 1,
}

// 버프 종류로 회복 효과 적용 여부를 판단한다.
eBuffType buffType = eBuffType.Heal;
bool isHeal = buffType == eBuffType.Heal;
```

기존 선언: [Enumeration.cs](../../Assets/Scripts/Utils/Enumeration.cs). 예제 실행은 미확인이다.

## Flags 조합 예제

여러 옵션을 함께 켤 때 사용한다. 각 항목은 `1`, `2`, `4`처럼 비트가 겹치지 않게 지정한다. 일반 상태 enum에는 붙이지 않는다.

```csharp
/// <summary>
/// 함께 적용할 수 있는 효과를 구분한다.
/// </summary>
[System.Flags]
public enum eEffectFlags
{
    None = 0,
    Heal = 1 << 0,
    Shield = 1 << 1,
}

eEffectFlags effects = eEffectFlags.Heal | eEffectFlags.Shield;
bool hasHeal = (effects & eEffectFlags.Heal) != eEffectFlags.None;
```

작성 예시이며 프로젝트에 선언된 타입은 아니다. 실행은 미확인이다.
