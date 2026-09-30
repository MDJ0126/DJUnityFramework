using Game;
using UnityEngine;

public class EditorPossessInput : MonoBehaviour
{
#if UNITY_EDITOR

    /// <summary>
    /// Play Mode 진입 시 자동 생성
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Initialize()
    {
        // 중복 생성 방지
        if (FindFirstObjectByType<EditorPossessInput>() != null)
            return;

        GameObject obj = new GameObject("[EditorPossessInput]");

        DontDestroyOnLoad(obj);

        obj.AddComponent<EditorPossessInput>();
    }

    /// <summary>
    /// 에디터 테스트용 단축키로 빙의 상태 전환 또는 해제
    /// </summary>
    private void Update()
    {
        // F8은 마지막 Pawn을 기준으로 빙의와 해제를 토글한다.
        if (Input.GetKeyDown(KeyCode.F8))
        {
            if (PlayerController.Instance.IsPossessed)
            {
                PlayerController.Instance.Unpossess();
            }
            else
            {
                PlayerController.Instance.Possess();
            }
        }

        // Escape는 현재 빙의 상태일 때만 해제한다.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PlayerController.Instance.IsPossessed)
            {
                PlayerController.Instance.Unpossess();
            }
        }
    }

#endif
}
