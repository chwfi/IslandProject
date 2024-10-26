using UnityEngine;

public interface IPoolable
{
    // 풀에서 가져올 때 호출되는 메서드
    void OnTakenFromPool();

    // 풀에 반환될 때 호출되는 메서드
    void OnReturnedToPool();
}