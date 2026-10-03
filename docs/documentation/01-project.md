# 폴더와 시스템 구조

기본 흐름은 GameMode → PlayerController → Pawn/Character입니다.

| 폴더: Assets/ 아래 | 역할 |
| --- | --- |
| Scripts/Management | 게임 모드 |
| Scripts/InGame | 캐릭터, 이동, 스킬, 버프, 능력치 |
| Scripts/Common | 카메라와 SpringArm |
| Scripts/UI/HUD | 추적 HUD |
| Scripts/Utils | 풀, 싱글톤, 캐시 |
| Scripts/Editor·Etc | 에디터·Inspector 보조 |
| Scenes·Input | Playground 씬과 입력 액션 |
| AddressablesResources/Prefabs | 자체 캐릭터·HUD 프리팹 |
| ArtResources | 외부 에셋 |

- Pawn.Awake: 부모 초기화·컴포넌트 조회. Start: 관리자 소유자 연결.
- Pawn.LateUpdate: 스킬·버프 갱신. Movement.FixedUpdate: 물리 이동.
- RobotKylePlayerController: 입력·조준. AnimationController: Animator·Foot IK.
- HUD 앵커는 PawnInfoAnchor·balloonAnchor. 기존 nameAnchor·healthBarAnchor는 없음.

전체 연결은 [인게임 다이어그램](07-diagrams.md), 아웃게임 상태는 [아웃게임 다이어그램](20-outgame-diagrams.md), 기능별 구현은 [기능 목록](08-features.md)을 봅니다. 씬의 실제 동작은 미확인입니다.
