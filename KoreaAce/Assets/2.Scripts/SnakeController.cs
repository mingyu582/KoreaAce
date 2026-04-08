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

    public int Gap = 10;

    public GameObject BodyPrefab;

    private List<GameObject> BodyParts = new List<GameObject>();
    private List<Vector3> PositionHistory = new List<Vector3>();

    public Transform[] points;
    public Transform moveTarget;

    public int currentIndex = 0;

    public Transform player;
    public bool isChase = false;

    public PlayerControllerV2 playerControllerV2;

    private void Start()
    {
        // 몸통 생성
        for (int i = 0; i < 8; i++)
            GrowSnake();

        currentIndex = 1;
        moveTarget = points[currentIndex];

        PositionHistory.Add(transform.position);
    }

    private void Update()
    {
        if (playerControllerV2.isDeadPlayer)
        {
            return;
        }
        // 몸통 이동 (계단 그대로 따라가게)
        // move forward
        transform.position += transform.forward * moveSpeed * Time.deltaTime;

        // steer
        float steerDirection = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * steerDirection * 3f * Time.deltaTime);

        // store position history
        PositionHistory.Insert(0, transform.position);

        // move body parts
        int index = 0;
        foreach (var body in BodyParts)
        {
            Vector3 point = PositionHistory[Mathf.Min(index * Gap, PositionHistory.Count - 1)];
            Vector3 moveDirection = point - body.transform.position;
            body.transform.position += moveDirection * moveSpeed * Time.deltaTime;
            body.transform.LookAt(point);
            index++;
        }

        // 추적 여부 판단
        float playerDistance = Vector3.Distance(player.position, transform.position);
        isChase = playerDistance < 10f &&
                  player.GetComponent<FloorCheck>().floorTag == FloorTagSnake;

        if (isChase)
            Chase();
        else
            Wander();

    }

    private void GrowSnake()
    {
        GameObject body = Instantiate(BodyPrefab);
        body.transform.position = transform.position;
        BodyParts.Add(body);
    }

    public void Wander()
    {
        Vector3 dir = (moveTarget.position - transform.position).normalized;

        // 이동
        transform.position = Vector3.MoveTowards(
            transform.position,
            moveTarget.position,
            moveSpeed * Time.deltaTime
        );

        // 이동 방향으로 회전
        if (dir != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 10f * Time.deltaTime);
        }

        // 도착 시 다음 포인트 선택
        if (Vector3.Distance(transform.position, moveTarget.position) < 0.2f)
        {
            List<int> possible = new List<int>();

            if (currentIndex - 1 >= 0)
                possible.Add(currentIndex - 1);

            if (currentIndex + 1 < points.Length)
                possible.Add(currentIndex + 1);

            currentIndex = possible[Random.Range(0, possible.Count)];
            moveTarget = points[currentIndex];
        }
    }

    public void Chase()
    {
        Vector3 dir = (player.position - transform.position).normalized;

        // 이동
        transform.position = Vector3.MoveTowards(
            transform.position,
            player.position,
            chaseSpeed * Time.deltaTime
        );

        // 플레이어 방향으로 회전
        if (dir != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 10f * Time.deltaTime);
        }

        //이동중에 이동풀렷을떄 어디갈지 계산 (풀렸을시 가장 가까운 포인트로 이동)
        float tableDis =10f;
        for (int i = 0; i < points.Length; i++)
        {
            if (tableDis > Vector3.Distance(transform.position, points[i].position))
            {
                tableDis = Vector3.Distance(transform.position, points[i].position);
                currentIndex = i;

                Debug.Log("현재인덱스 " + currentIndex);
            }
        }


        moveTarget = points[currentIndex];
    }
}