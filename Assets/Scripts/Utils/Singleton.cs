public abstract class Singleton<T> where T : Singleton<T>, new()
{
    private static T instance = null;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                // 최초 접근 시 인스턴스를 만들고 파생 클래스의 초기화 순서를 보장한다.
                instance = new T();
                instance.Initialize();
            }
            return instance;
        }
    }

    /// <summary>
    /// 인스턴스 수명이 끝날 때 파생 클래스의 정리 처리 호출
    /// </summary>
    ~Singleton()
    {
        Release();
    }

    public static bool IsLive => instance != null;
    /// <summary>
    /// 싱글톤 최초 생성 시 필요한 데이터 초기화
    /// </summary>
    protected abstract void Initialize();
    /// <summary>
    /// 싱글톤 소멸 시 사용 중인 데이터 정리
    /// </summary>
    protected abstract void Release();
}
