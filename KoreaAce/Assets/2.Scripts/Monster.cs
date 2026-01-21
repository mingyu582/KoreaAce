using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum MonsterState { None = -1, Idle = 0, Wander, }

public class Monster : MonoBehaviour
{
    private MonsterState enemyState = MonsterState.None;

    //private Status;
    private NavMeshAgent navMeshAgent;

    private void Awake()
    {
        navMeshAgent= GetComponent<NavMeshAgent>();

        navMeshAgent.updateRotation = false;
    }
}
