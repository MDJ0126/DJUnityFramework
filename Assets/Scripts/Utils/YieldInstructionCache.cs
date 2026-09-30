using System.Collections.Generic;
using UnityEngine;

public static class YieldInstructionCache
{
    public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
    public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();
    private static readonly Dictionary<float, WaitForSeconds> waitForSeconds = new Dictionary<float, WaitForSeconds>();

    /// <summary>
    /// 지정한 대기 시간에 해당하는 WaitForSeconds를 캐시에서 가져오기
    /// </summary>
    public static WaitForSeconds WaitForSeconds(float seconds)
    {
        WaitForSeconds wfs;
        // 같은 시간 값은 새로 생성하지 않고 기존 인스턴스를 재사용한다.
        if (!waitForSeconds.TryGetValue(seconds, out wfs))
            waitForSeconds.Add(seconds, wfs = new WaitForSeconds(seconds));
        return wfs;
    }
}
