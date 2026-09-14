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

    private void Update()
    {
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
    }

#endif
}