# 주석·줄바꿈 규칙

- 짧거나 보통 길이의 호출은 한 줄. 읽기 어려울 때만 줄바꿈.
- summary는 여는 태그·설명·닫는 태그의 3줄.
- 주석은 한국어 '~한다.' 톤으로 역할·전제·부작용을 설명.
- 내부 주석은 순서·계산·수명 처리의 이유를 설명.
- 빈 param/returns, 함수명 반복, 대화체, 미검증 보장은 금지.

```csharp
float distance = Vector3.Distance(target.position, _targetCamera.transform.position);

/// <summary>
/// 이전 Pawn의 구독을 해제하고 새 Pawn을 연결한다.
/// </summary>
/// <param name="pawn">연결할 Pawn. null이면 연결을 해제한다.</param>

// 풀 재사용 시 이전 이벤트가 남지 않도록 먼저 구독을 해제한다.
// TODO: 종료 중 목록 제거의 재현을 확인한다.
```

위 주석은 패턴 예시입니다. 구현이 바뀌면 주석도 함께 고칩니다.
