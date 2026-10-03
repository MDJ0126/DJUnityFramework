# 네이밍 컨벤션

| 대상 | 규칙 | 예 |
| --- | --- | --- |
| Class·프로퍼티·함수 | PascalCase | PlayerController, IsMoving, SetOwner |
| private 필드 | _camelCase | _owner |
| public·protected 필드, 인자·지역 변수 | camelCase | owner, deltaTime |
| bool | is·has·can 접두사 | _isMoving, IsMoving, CanJump |
| interface | i + PascalCase | iManager |
| enum | e + PascalCase | eBuffType |
| 상수 | UPPER_SNAKE_CASE | GROUND_STICK_SPEED |

4칸 들여쓰기, 다음 줄 중괄호를 사용합니다.

## bool 이름 예시

```csharp
private bool _isMoving;
public bool isMoving;
public bool IsMoving { get; private set; }

public bool CanJump()
{
    return _isMoving;
}
```

이 예시는 이름 형태를 설명합니다. 실제 점프 조건을 뜻하지 않습니다.

기존 예외는 보존합니다: Initalize, PawnInfoAnchor, private Inspector 필드. 이름 변경 전 호출부·override·씬·프리팹을 확인합니다. 네임스페이스는 주변 코드에 맞춥니다.
