using TMPro;
using UnityEngine;
using StarterAssets;

public class PlayerControllerV2 : MonoBehaviour
{
    public Transform cam;
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


    public TMP_Text InteractText;
    public GameObject InteractBtn;
    public FloorCheck floorCheck;

    public bool isInteractPass = false;
    public bool isDeadPlayer = false;

    public GameObject gameOverPanel;

    public Transform clearPosition;

    void Update()
    {
        ShootRay();
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

    

    public void ShootRay()
    {

        Ray ray = new Ray(transform.position + new Vector3(0, 1.375f, 0), cam.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 2f, 1 << 9)) //layermask 가 9일때 (note 일때)
        {
            InteractText.text = "노트 보기";
            InteractBtn.SetActive(true);

            if (isInteractPass || Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("SLDFKJFJFFJFJJ");
                if (personController.enabled)  //메모가 안켰다면
                {
                    hit.collider.GetComponent<ReadNotes>().ReadNote();
                }
                else
                {
                    hit.collider.GetComponent<ReadNotes>().ExitButton();
                }
                UsedInteract();
            }
        }
        else
        {
            InteractBtn.SetActive(false);
            InteractText.text = "";
        }
    }
    public void UsedInteract()
    {
        InteractText.text = "";
        InteractBtn.SetActive(false);
        isInteractPass = false;
    }
    public void ClickIntaract()
    {
        isInteractPass = true;
    }


    void CheckGround()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, rayDistance, groundLayer);
    }

    private void Start()
    {
        //GameOver();
    }


    public void SnakeHit()
    {
        Debug.Log("@@@@");
        isDeadPlayer = true;
        personController.enabled = false;
        floorCheck.enabled = false;
        floorCheck.floorTag = "00";
        GameOver();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Clear"))
        {
            GameClear();
        }
    }

    public void GameClear()
    {
        personController.enabled = false;
        transform.position = clearPosition.position;
    }

    public void GameOver()
    {
        isGameOver = true;
        blink.StartBlink();
        Invoke("Respone", 2f);
    }

    public void Respone()
    {
        isDeadPlayer = false;
        isGameOver = false;
        personController.enabled = false;
        transform.position = spawnPoint.position;

        blink.StopBlink();
        SoundManager.Instance.SFXPlay(deadClip);
        personController.enabled = true;

        floorCheck.enabled = true;
        gameOverPanel.SetActive(true);
    }

    public void SetFalseGameOverPanel()
    {
        gameOverPanel.SetActive(false);
    }
}
