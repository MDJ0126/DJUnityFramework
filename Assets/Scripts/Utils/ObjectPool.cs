using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    // 30초마다 오래 사용하지 않은 오브젝트를 삭제한다.
    private const float REFRESH_TIME_PER_SECONDS = 30f;

    private class PoolItem
    {
        public GameObject gameObject;

        public bool isActive => gameObject.activeSelf;

        public DateTime lastActiveTime = DateTime.Now;
    }

    #region Inspector

    public GameObject original;
    public int count = 10;

    #endregion;

    private List<PoolItem> _pool = new();

    private DateTime _lastRefreshedTime = DateTime.Now;

    /// <summary>
    /// 원본을 비활성화하고 인스펙터에 설정한 수만큼 풀을 미리 생성
    /// </summary>
    private void Start()
    {
        original.SetActive(false);

        int tempCount = count;
        count = 0;
        for (int i = 0; i < tempCount; i++)
        {
            Create();
        }
    }

    /// <summary>
    /// 원본을 복제하여 풀의 기본 트랜스폼 상태로 등록
    /// </summary>
    private PoolItem Create()
    {
        GameObject go = Instantiate(original);
        go.SetActive(false);
        go.transform.SetParent(transform);
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = Vector3.one;
        go.name = $"{original.name} {_pool.Count}";
        PoolItem item = new PoolItem { gameObject = go };
        _pool.Add(item);
        count++;
        return item;
    }

    /// <summary>
    /// 비활성 오브젝트를 가져오고 부족하면 새로 생성
    /// </summary>
    public GameObject Get()
    {
        var item = _pool.Find(poolItem => !poolItem.gameObject.activeSelf);
        if (item == null) item = Create();
        item.lastActiveTime = DateTime.Now;
        AutoReleaseMemory();
        return item.gameObject;
    }

    /// <summary>
    /// 비활성 오브젝트에서 요청한 컴포넌트를 가져오기
    /// </summary>
    public T Get<T>() where T : Component
    {
        var item = _pool.Find(poolItem => !poolItem.gameObject.activeSelf);
        if (item == null) item = Create();
        item.lastActiveTime = DateTime.Now;
        AutoReleaseMemory();
        return item.gameObject.GetComponent<T>();
    }

    /// <summary>
    /// 일정 시간마다 장기간 사용하지 않은 비활성 오브젝트 정리
    /// </summary>
    private void AutoReleaseMemory()
    {
        var nowTime = DateTime.Now;
        if (_lastRefreshedTime.AddSeconds(REFRESH_TIME_PER_SECONDS) < nowTime)
        {
            _lastRefreshedTime = nowTime;
            // 삭제 중 인덱스가 밀리지 않도록 목록의 뒤에서부터 검사한다.
            for (int i = _pool.Count - 1; i >= 0; --i)
            {
                var item = _pool[i];
                if (!item.gameObject.activeSelf && item.lastActiveTime.AddSeconds(REFRESH_TIME_PER_SECONDS) < nowTime)
                {
                    _pool.RemoveAt(i);
                    Destroy(item.gameObject);
                    count--;
                }
            }
        }
    }
}
