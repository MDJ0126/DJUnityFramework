# Class 작성

- 관리·공유 상태는 class. GameObject·Inspector·Unity 메시지가 필요하면 MonoBehaviour.
- 역할은 하나로 제한. 내부 상태는 private, 조회는 프로퍼티.
- 이름·파일은 PascalCase. 부모 초기화와 정리를 보존.

제안 예제이며 컴파일·실행은 미확인입니다.

```csharp
/// <summary>
/// 전달받은 값을 누적한다.
/// </summary>
public class ExampleCounter
{
    private int _value;
    public int Value => _value;

    /// <summary>
    /// 현재 값에 전달한 값을 더한다.
    /// </summary>
    public void Add(int value)
    {
        _value += value;
    }
}
```
