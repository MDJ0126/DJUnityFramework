# Struct 작성

- 독립된 값 묶음에 사용. 공유 상태·이벤트·수명 관리는 class 검토.
- 대입·인자 전달은 값 복사. 참조 필드의 객체는 깊은 복사가 아님.
- 단위·기본값·계산할 필드를 명확히 설명.

현재 Status는 damage·defence·maxHp를 묶습니다.

```csharp
Status original = new Status { damage = 10, defence = 2, maxHp = 100 };
Status copied = original;
copied.damage = 20;
```

copied 변경은 original의 int 필드와 별개입니다. 예제 실행은 미확인입니다.
