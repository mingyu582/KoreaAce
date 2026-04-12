using TMPro;
using UnityEngine;
using StarterAssets;
using System.Collections;
using UnityEngine.UI;

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

    public float maxAirTime = 2f; //  5초 지나면 즉사

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

    public GameObject lastSafe;
    public Light directionalLight;
    public Text clearText;
    public GameObject clearPanel;
    private bool isClear = false;
    public GameObject menuBtn;

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

            if (airTime > 2f)
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
        else if (other.gameObject.CompareTag("Safe"))
        {
            floorCheck.floorTag = "Safe";
        }
    }

    public void GameClear()
    {
        isClear = true;
        personController.enabled = false;
        lastSafe.SetActive(false);
        clearPanel.SetActive(true);
        transform.position = spawnPoint.position;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

        StartCoroutine(sunRise());
        Invoke("GameClear2", 4f);
    }

    IEnumerator sunRise()
    {
        float time = 0f;

        while (time < 3f)
        {
            time += Time.deltaTime;

            float t = time / 3f;
            directionalLight.intensity = Mathf.Lerp(1f, 50f, t);

            yield return null;
        }

        // 정확한 값 보정
        directionalLight.intensity = 50f;

        directionalLight.color = new Color32(205, 60, 21, 255);
    }

    public void GameClear2()
    {
        clearText.text = "당신은 200층 타워를 무사히 탈출했다.";
        Invoke("GameClear3", 2f);
    }

    public void GameClear3()
    {
        clearText.text = "하어덕 성에서의 악몽은 이제 끝났다...";
        Invoke("BtnSet", 4f);
    }

    public void BtnSet()
    {
        menuBtn.SetActive(true);
    }

    public void GameOver()
    {
        if (isClear)
        {
            return;
        }
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
