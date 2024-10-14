using UnityEngine;
using UnityEngine.AI;

public abstract class MoveComponent : BaseComponent
{
    [Header("Speed")]
    [SerializeField] protected float _moveSpeed; // 이동속도

    public CharacterController CharacterControllerCompo { get; private set; }
    public NavMeshAgent NavAgentCompo { get; private set; }
    
    protected virtual void Awake()
    {
        CharacterControllerCompo = transform.GetComponent<CharacterController>();
        NavAgentCompo = transform.GetComponent<NavMeshAgent>();
    }

    public void StopImmediately()
    {
        NavAgentCompo.isStopped = true;
    }

    public abstract void OnMove(); // 여기서 움직임 로직 실행
}