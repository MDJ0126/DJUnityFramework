# 이벤트 작성

- 구독과 해제를 같은 수명 범위에 둔다. `OnEnable` 구독은 `OnDisable`에서 해제한다.
- 연결 대상을 바꾸기 전에 이전 대상의 구독을 해제한다.
- 해제가 필요하면 익명 람다 대신 이름 있는 함수를 사용한다.
- 이벤트는 선언한 쪽에서 호출한다. 구독자는 전달된 값으로 화면·상태를 갱신한다.

## 구독·해제 예제

`Game` 네임스페이스의 MonoBehaviour 내부 예제다. `_status`는 활성화 전에 연결한다.

```csharp
private StatusInfo _status;

private void OnEnable()
{
    if (_status == null) return;
    _status.OnChangedHp += OnChangedHp;
}

private void OnDisable()
{
    if (_status == null) return;
    _status.OnChangedHp -= OnChangedHp;
}

/// <summary>
/// 변경된 체력을 콘솔에 표시한다.
/// </summary>
private void OnChangedHp(StatusInfo statusInfo)
{
    UnityEngine.Debug.Log(statusInfo.hp);
}
```

현재 [FollowPawnInfo](../../Assets/Scripts/UI/HUD/FollowPawnInfo.cs)는 `SetPawn`에서 구독하고 `OnDisable`에서 해제한다. 위 예제는 작성 패턴이며 기존 구현과 같다는 뜻은 아니다. 실행은 미확인이다.
