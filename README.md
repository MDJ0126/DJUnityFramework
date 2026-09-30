<div align="center">

# DJ Unity Framework

**게임 개발에 필요한 기능을 직접 만들고 쌓아가는 개인용 Unity 프레임워크**

![Unity](https://img.shields.io/badge/Unity-6000.0.83f1-000000?style=flat-square&logo=unity&logoColor=white)
![Purpose](https://img.shields.io/badge/Purpose-Personal_Framework_%26_Code_Samples-6C63FF?style=flat-square)

![DJ Unity Framework](docs/images/thumbnail.png)

</div>

## 소개

프로젝트를 시작할 때 반복해서 사용하는 기능과 구조를 정리하고,  
직접 구현하며 학습한 내용을 하나의 재사용 가능한 기반으로 만드는 프로젝트입니다.

개인 프로젝트의 기반으로 사용하는 동시에 **샘플 코드 제출 및 기술 검토 자료**로도 활용합니다.  
완성된 하나의 게임을 제공하는 저장소가 아니라, 기능별 구현과 설계 방식을 보여주는 데 목적이 있습니다.

## 목표

- 자주 사용하는 시스템을 모듈화하여 재사용하기
- 새로운 기능을 자유롭게 실험하고 검증하기
- 나만의 개발 방식과 코드 스타일을 꾸준히 정리하기
- 실제 프로젝트에 빠르게 적용할 수 있는 기반 만들기

## 프로젝트 환경

- **Unity:** 6000.0.83f1
- **Render Pipeline:** Universal Render Pipeline (URP) 17.0.4
- **Input:** Input System 1.19.0
- **Asset Management:** Addressables 2.9.1

## 개발된 기능

### 캐릭터 및 플레이어 제어

- Pawn 빙의 및 빙의 해제
- 카메라 방향을 기준으로 한 이동과 달리기, 점프
- 지면 및 경사면 판정, 코요테 타임 처리
- 이동 상태에 따른 캐릭터 회전과 애니메이션 파라미터 갱신
- 화면 중앙 Raycast를 이용한 에임 타겟 및 Aim Rig 가중치 제어

### 카메라

- Spring Arm 기반 3인칭 카메라 위치 및 회전 설정
- 마우스 입력을 이용한 Yaw/Pitch 궤도 회전
- Raycast와 SphereCast를 이용한 카메라 장애물 충돌 처리
- 장애물 진입 시 즉시 축소하고 이탈 시 부드럽게 원래 거리로 복귀

### 애니메이션 및 Foot IK

- 이동 속도와 방향을 이용한 애니메이션 블렌드 값 갱신
- 정지 상태에서 양발의 지면 위치와 경사를 반영하는 Foot IK
- 발의 높이 차이에 따른 캐릭터 몸체 높이 보정

### HUD

- 월드 오브젝트를 화면 좌표로 변환하여 추적하는 HUD
- 이름표와 말풍선 HUD 부착 및 해제
- 오브젝트 풀을 이용한 추적 HUD 재사용
- 씬 뷰에서 HUD 앵커 위치 및 이름 표시

### 게임플레이 기반 구조

- Pawn을 기준으로 한 스킬 및 버프 관리 구조
- 스킬 실행과 지속형 버프 갱신을 위한 확장 클래스
- 스테이터스 조합을 위한 덧셈 및 뺄셈 연산자
- Game Mode를 통한 기본 Pawn 자동 빙의

### 공통 유틸리티 및 에디터 도구

- MonoBehaviour 및 일반 클래스용 싱글톤
- 동적 생성과 미사용 오브젝트 정리를 지원하는 오브젝트 풀
- Coroutine Yield Instruction 캐시
- 카메라, 레이어, 확률, 씬 오브젝트 검색 관련 유틸리티
- 인스펙터 읽기 전용 및 표시 이름 변경 어트리뷰트
- 씬 뷰에서 Spring Arm 카메라 위치와 회전을 편집하는 핸들
- 에디터 테스트용 Pawn 빙의 단축키
- 지면 크기에 맞춘 머티리얼 텍스처 타일링

## 시작하기

1. 저장소를 내려받습니다.
2. Unity Hub에서 프로젝트 폴더를 추가합니다.
3. Unity `6000.0.83f1` 버전으로 프로젝트를 엽니다.

---

> 개인 프레임워크이자 코드 샘플 모음으로, 필요한 기능을 독립적으로 구현하고 지속적으로 개선합니다.
