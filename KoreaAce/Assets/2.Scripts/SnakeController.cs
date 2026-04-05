using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SnakeController : MonoBehaviour
{
    public string FloorTagSnake;
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

    //  포인트 (A, B, C 순서대로 넣기)
    public Transform[] points;
    public Transform moveTarget;

    private int currentIndex = 0;

    public Transform player;
    public bool isChase = false;

    private void Start()
    {
        // 몸통 생성
        for (int i = 0; i < 6; i++)
            GrowSnake();

        //  시작 위치 랜덤
        currentIndex = 1;
        moveTarget = points[currentIndex];
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * transform.rotation.z * steerSpeed * Time.deltaTime);

        PositionHistory.Insert(0, transform.position);

        int index = 0;
        foreach (var body in BodyParts)
        {
            Vector3 point = PositionHistory[Mathf.Min(index * Gap, PositionHistory.Count - 1)];
            Vector3 moveDirection = point - body.transform.position;
            body.transform.position += moveDirection * bodySpeed * Time.deltaTime;
            body.transform.LookAt(point);
            index++;
        }

        float playerDistance = Vector3.Distance(player.position, transform.position);
        isChase = playerDistance < 10f && player.gameObject.GetComponent<FloorCheck>().floorTag == FloorTagSnake;

        if (isChase)
            Chase();
        else
            Wander();
    }

    private void GrowSnake()
    {
        GameObject body = Instantiate(BodyPrefab);
        BodyParts.Add(body);
    }

    //  핵심: 한 칸씩 랜덤 이동
    public void Wander()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            moveTarget.position,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, moveTarget.position) < 0.2f)
        {
            List<int> possible = new List<int>();

            // 왼쪽 이동 가능
            if (currentIndex - 1 >= 0)
                possible.Add(currentIndex - 1);

            // 오른쪽 이동 가능
            if (currentIndex + 1 < points.Length)
                possible.Add(currentIndex + 1);

            // 랜덤 선택
            currentIndex = possible[Random.Range(0, possible.Count)];
            moveTarget = points[currentIndex];
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
