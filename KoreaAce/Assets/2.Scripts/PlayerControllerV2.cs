using TMPro;
using UnityEngine;
using StarterAssets;

public class PlayerControllerV2 : MonoBehaviour
{
    private bool isGameOver = false;
    public DeadBlink blink;
    public Transform spawnPoint;
    public FirstPersonController personController;
    public AudioClip deadClip;

    public float minFallHeight = 3f;
    public float damageMultiplier = 5f;

    public float maxAirTime = 5f; //  5초 지나면 즉사

    private float highestY;
    public bool isGrounded;
    private float airTime = 0f;

    public LayerMask groundLayer;
    public float rayDistance = 1.2f;

    void Update()
    {
        CheckGround();

        if (isGameOver)
            return;
        if (!isGrounded)
        {
            airTime += Time.deltaTime;

            //  5초 이상 공중 → 즉사
            if (airTime >= maxAirTime)
            {
                GameOver();
                airTime = 0f;
                return;
            }
        }
        else
        {
            // 착지 시 낙뎀 계산

            if (airTime > 3f)
            {
                GameOver();
            }

            // 초기화
            airTime = 0f;
        }
    }

    void CheckGround()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, rayDistance, groundLayer);
    }

    private void Start()
    {
        //GameOver();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Snake")
        {

        }
    }

    public void GameOver()
    {
        isGameOver = true;
        blink.StartBlink();
        Invoke("Respone", 2f);
    }

    public void Respone()
    {
        isGameOver = false;
        personController.enabled = false;
        transform.position = spawnPoint.position;

        blink.StopBlink();
        SoundManager.Instance.SFXPlay(deadClip);
        personController.enabled = true;
    }
}
