using UnityEngine;

public abstract class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance = null;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                // 씬에 이미 배치된 동일 타입 컴포넌트를 최초 접근 시 찾아 캐싱한다.
                instance = Object.FindFirstObjectByType(typeof(T)) as T;
            }
            return instance;
        }
    }

    /// <summary>
    /// 캐싱한 싱글톤 오브젝트가 제거되면 참조 해제
    /// </summary>
    protected virtual void OnDestroy()
    {
        if (IsLive) instance = null;
    }

    public static bool IsLive => instance != null;
}
