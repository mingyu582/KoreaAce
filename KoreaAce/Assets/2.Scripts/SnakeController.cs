using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SnakeController : MonoBehaviour
{
    public int snakeNum;
    public float moveSpeed;
    public float chaseSpeed;
    public NavMeshAgent navMeshAgent;
    public Transform target;
    public float steerSpeed = 100f;
    public int Gap = 10;
    public float bodySpeed = 5f;

    public GameObject BodyPrefab;

    private List<GameObject> BodyParts = new List<GameObject>();
    private List<Vector3> PositionHistory = new List<Vector3>();

    private MonsterState monsterState = MonsterState.None;

    //move

    public Transform pointA;
    public Transform pointB;

    private Transform moveTarget;

    public Transform player;
    public bool isChase = false;    

    private void Start()
    {
        //navMeshAgent.updateRotation = true;
        GrowSnake();
        GrowSnake();
        GrowSnake();
        GrowSnake();
        GrowSnake();
        GrowSnake();

        //move

        moveTarget = pointB;
    }

    private void Update()
    {
        //transform.position += transform.forward * moveSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up * transform.rotation.z * steerSpeed * Time.deltaTime);  ///주변을 돌아다니고, 플레이어가 나타나면 플레이어 바라보는 것 까지 하기

        PositionHistory.Insert(0, transform.position);

        int index = 0;
        foreach (var body in BodyParts)
        {
            Vector3 point = PositionHistory[Mathf.Min(index * Gap,PositionHistory.Count-1)];
            Vector3 moveDirection = point - body.transform.position;
            body.transform.position += moveDirection * bodySpeed * Time.deltaTime;
            body.transform.LookAt(point);
            index++;
        }

        float playerDistance = Vector3.Distance(player.position, transform.position);
        if (playerDistance < 10f)
        {
            isChase = true;
        }
        else
        {
            isChase = false;
        }
        //move
        // 이동
        if (isChase)
        {
            Chase();
        }
        else
        {
            Wander();
        }
    }

    private void GrowSnake()
    {
        GameObject body = Instantiate(BodyPrefab);
        BodyParts.Add(body);
    }

    public void Wander()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            moveTarget.position,
            moveSpeed * Time.deltaTime
        );

        // 도착하면 방향 전환
        if (Vector3.Distance(transform.position, moveTarget.position) < 0.2f)
        {
            moveTarget = moveTarget == pointA ? pointB : pointA;
        }
    }

    public void Chase()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            player.position,
            chaseSpeed * Time.deltaTime
        );
    }

    

    /*private void OnEnable()
    {
        StartCoroutine(Wander());
    }

    private IEnumerator Wander()
    {
        navMeshAgent.isStopped = false;



        float currentTime = 0;
        float maxTime = 10;

        navMeshAgent.speed = moveSpeed;

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

    private void CalculateDistanceToTargetAndSelectState()
    {
        if (target == null)
            return;

        *//*if (player.isHiding)
            return;*//*

        float distance = Vector3.Distance(target.position, transform.position);

        *//*if (distance <= targetRecognitonRange && GameManager.Instance.isClearUnderLock)
        {
            ChangeState(MonsterState.Pursuit);
        }
        else if (distance >= pursuitLimitRange)
        {
            ChangeState(MonsterState.Wander);
        }*//*
    }

    public void ChangeState(MonsterState newState)
    {
        if (monsterState == newState)
            return;

        StopCoroutine(monsterState.ToString());

        monsterState = newState;

        StartCoroutine(monsterState.ToString());
    }*/
}
