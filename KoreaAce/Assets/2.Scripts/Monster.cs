using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum MonsterState { None = -1, Idle = 0, Wander, Pursuit, }

public class Monster : MonoBehaviour
{
    [Header("Pursuit")]
    [SerializeField]
    private float targetRecognitonRange = 8;
    [SerializeField]
    private float pursuitLimitRange = 10;
    private MonsterState monsterState = MonsterState.None;

    //private Status;
    public float walkSpeed = 5;
    public float runSpeed = 10;
    private NavMeshAgent navMeshAgent;
    public Transform target;
    //animation
    public Animator animator;
    //player
    public PlayerController player;

    //Hide
    public float viewAngle = 90f;

    public bool canSeePlayer;

    //Drop
    public bool isDrop = false;    


    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.updateRotation = false;
    }

    private void Start()
    {
        animator.SetBool("isWalk", true);
    }

    private void Update()
    {
        
    }

    private void OnEnable()
    {
        if (navMeshAgent == null)
        {
            return;
        }
        ChangeState(MonsterState.Idle);
    }

    private void OnDisable()
    {
        StopCoroutine(monsterState.ToString());

        monsterState = MonsterState.None;
    }

    public void ChangeState(MonsterState newState)
    {
        if (monsterState == newState)
            return;

        StopCoroutine(monsterState.ToString());

        monsterState = newState;

        StartCoroutine(monsterState.ToString());
    }

    private IEnumerator Idle()
    {
        /*StartCoroutine("AutoChangeFromIdleToWander");
*/

        while (true)
        {
            CalculateDistanceToTargetAndSelectState();
            yield return null;
        }
    }
/*
    private IEnumerator AutoChangeFromIdleToWander()
    {
        int changeTime = Random.Range(1, 5);

        yield return new WaitForSeconds(changeTime);

        ChangeState(MonsterState.Wander);
    }*/

    private IEnumerator Wander()
    {
        yield return new WaitForSeconds(3f);

        navMeshAgent.isStopped = false;
        //플레이어 최초 발견시 3초뒤 움직임
        animator.SetBool("Walk", true);
        animator.SetBool("isChase", false); 
        animator.SetBool("isMiss", false);



        float currentTime = 0;
        float maxTime = 10;

        navMeshAgent.speed = walkSpeed;

        navMeshAgent.SetDestination(CalculateWanderPosition());

        Vector3 to = new Vector3(navMeshAgent.destination.x, 0, navMeshAgent.destination.z);
        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);
        transform.rotation = Quaternion.LookRotation(to - from);

        while (true)
        {
            currentTime += Time.deltaTime;
            to = new Vector3(navMeshAgent.destination.x, 0, navMeshAgent.destination.z);
            from = new Vector3(transform.position.x, 0, transform.position.z);
            if ((to - from).sqrMagnitude < 0.01f || currentTime >= maxTime)
            {
                ChangeState(MonsterState.Idle);
            }

            CalculateDistanceToTargetAndSelectState();

            yield return null;
        }
    }

    private Vector3 CalculateWanderPosition()
    {
        float wanderRadius = 10;
        int wanderJitter = 0;
        int wanderJitterMin = 0;
        int WanderJitterMax = 360;

        Vector3 rangePosition = Vector3.zero;
        Vector3 rangeScale = Vector3.one * 100.0f;

        wanderJitter = Random.Range(wanderJitterMin, WanderJitterMax);
        Vector3 targetPosition = transform.position + SetAngle(wanderRadius, wanderJitter);

        targetPosition.x = Mathf.Clamp(targetPosition.x, rangePosition.x - rangeScale.x * 0.5f, rangePosition.x + rangeScale.x * 0.5f);
        targetPosition.y = 0.0f;
        targetPosition.z = Mathf.Clamp(targetPosition.z, rangePosition.z - rangeScale.z * 0.5f, rangePosition.z + rangeScale.z * 0.5f);

        return targetPosition;
    }

    private Vector3 SetAngle(float radius, int angle)
    {
        Vector3 position = Vector3.zero;

        position.x = Mathf.Cos(angle) * radius;
        position.z = Mathf.Sin(angle) * radius;

        return position;
    }

    private IEnumerator Pursuit()
    {
        animator.SetBool("isChase", true);
        while (true)
        {
            HideCheck();
            navMeshAgent.speed = runSpeed;


            float distance = Vector3.Distance(target.position, transform.position);
            navMeshAgent.stoppingDistance = 1.3f;
            if (player.isHiding)
            {
                float dis = Vector3.Distance(transform.position, player.tablePos);
                
                if (dis < 3f) //플레이어 근처에 도착했다면
                {
                    navMeshAgent.isStopped = true;
                    animator.SetBool("isMiss", true);
                    Debug.Log("플렝이ㅓ 근처에 도착");
                    isDrop = false;
                    int ranNum = Random.Range(0, 4);
                    Debug.Log("랜덤넘버" + ranNum);
                    if (ranNum == 0)
                    {
                        animator.SetBool("isAttack", true);
                        Invoke("PlayerDead", 3f);
                        yield break;
                    }
                    ChangeState(MonsterState.Wander);
                    yield break;
                }
                else
                {
                    navMeshAgent.SetDestination(player.tablePos);
                    yield return null;
                }
            }

            navMeshAgent.SetDestination(target.position);

            Vector3 dir = (transform.position - target.position).normalized;
            Vector3 stopPos = target.position + dir * navMeshAgent.stoppingDistance;

            if (distance > navMeshAgent.stoppingDistance)
            {
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(stopPos);
            }
            else
            {
                navMeshAgent.isStopped = true;
            }

            if (distance < 2.4f && !player.isHiding)
            {
                animator.SetBool("isAttack", true);
                player.isDead();
            }
            else
            {
                LookRotationToTarget();
                animator.SetBool("isAttack", false);
            }

            

            

            CalculateDistanceToTargetAndSelectState();

            yield return null;
        }
    }

    public void PlayerDead()
    {
        player.isDead();
        GameManager.Instance.OffSound();
    }

    private void LookRotationToTarget()
    {
        Vector3 to = new Vector3(target.position.x, 0, target.position.z);
        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);

        transform.rotation = Quaternion.LookRotation(to - from);
    }

    private void CalculateDistanceToTargetAndSelectState()
    {
        if (target == null)
            return;

        if (isDrop)
            return;

        if (player.isHiding)
            return;

        float distance = Vector3.Distance(target.position, transform.position);

        //Debug.Log(distance +"sldfkj"+ targetRecognitonRange);

        if (distance <= targetRecognitonRange && GameManager.Instance.isClearUnderLock)
        {
            Debug.Log("@");
            animator.SetBool("isChase", true);
            ChangeState(MonsterState.Pursuit);
        }
        else if (distance >= pursuitLimitRange)
        {
            //Debug.Log("@@@");
            animator.SetBool("isChase", false);
            ChangeState(MonsterState.Wander);
        }
    }

    public void HideCheck()
    {
        if (!player.isHiding) //안숨었다면 실행하지 않는다
            return;

        
        int ranNum = Random.Range(0, 3);
        if (ranNum > 0)
        {

        }

        // 다 통과 → 시야에 보임
        canSeePlayer = true;
    }
}
