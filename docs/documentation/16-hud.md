# HUD 만들기

1. FollowHUD를 상속하고 프리팹의 UI 참조를 연결합니다.
2. 대상 추적은 SetTarget, 데이터 표시는 별도 연결 함수로 처리합니다.
3. 풀에서 꺼내 대상·데이터를 준비한 뒤 Show합니다.
4. Hide/OnDisable에서 이전 데이터·구독을 정리합니다.

유효한 pool·pawn·PawnInfoAnchor가 있고 풀 초기화가 끝난 호출 예시입니다. 실행은 미확인입니다.

```csharp
FollowPawnInfo hud = pool.Get<FollowPawnInfo>();
hud.SetTarget(pawn.PawnInfoAnchor);
hud.SetPawn(pawn);
hud.Show();
```

- Get은 활성화하지 않음. 현재 HUDManager 부착 함수는 SetPawn 호출 없음.
- 재사용 전에 이전 이벤트 해제. 데이터 변경의 이벤트 발행도 확인.
- 거리 숨김은 스케일 0. 벽 가림 판정과는 별개.
