using TMPro;
using UnityEngine;
using StarterAssets;

public class PlayerController : MonoBehaviour
{
    public Transform cam;

    public TMP_Text InteractText;
    public GameObject InteractBtn;

    public Transform monsterTrans;
    public FirstPersonController moveController;

    //Hide
    public GameObject HideBtn;
    private Vector3 hidePos;
    private bool isCanHide = false;
    public bool isHiding = false;
    public Vector3 tablePos;
    private bool isBed = false;
    //monster
    public Monster monsterScript;
    //item
    public GameObject[] itemsHUD;
    private bool isUseHandle = false;
    private bool isUseUnderGroundLock = false;
    private bool isUseSecretLock = false;
    private bool isUseMasterLock = false;
    private int Durabilitywood = 4; //내구도
    private int screwDurability = 4; //내구도
    public Rigidbody FrameRigid;

    public GameObject UnderLock;

    public bool isInteractPass = false;
    public Transform standPosition;

    public GameObject deadCanvas;

    private void Update()
    {
        ShootRay();
        float distance = Vector3.Distance(hidePos, transform.position);
        if (distance > 3f)
        {
            HideBtn.SetActive(false);
            isCanHide = false;
        }
    }

    public void UsedInteract()
    {
        InteractText.text = "";
        InteractBtn.SetActive(false);
        isInteractPass = false;
    }

    public void ShootRay()
    {
        Ray ray = new Ray(transform.position + new Vector3(0, 1.7f, 0), cam.forward);
        RaycastHit hit;
            
        if (Physics.Raycast(ray, out hit, 2f, 1 << 6)) //layermask 가 6일때 (Door일때)
        {
            InteractText.text = "열기";
            InteractBtn.SetActive(true);
            isInteractPass = true;
            if (isInteractPass)
            {
                hit.collider.GetComponent<Doors>().DoorOpen();
                UsedInteract();
            }
        }
        else if (Physics.Raycast(ray, out hit, 2f, 1 << 8)) //layermask 가 8일때 (item일때)
        {
            string itemName = "item";
            switch (hit.collider.gameObject.tag)
            {
                case "Crowbar":
                    itemName = "쇠지렛대";
                    break;
                case "Hammer":
                    itemName = "망치";
                    break;
                case "Screwdriver":
                    itemName = "드라이버";
                    break;
                case "Handle":
                    itemName = "핸들";
                    break;
                case "UnderGroundKey":
                    itemName = "지하실키";
                    break;
                case "SecretKey":
                    itemName = "시크릿키";
                    break;
                case "MasterKey":
                    itemName = "마스터키";
                    break;
            }

            InteractText.text = itemName + "획득";
            InteractBtn.SetActive(true);
            if (isInteractPass || Input.GetKeyDown(KeyCode.E))
            {
                switch (hit.collider.gameObject.tag)
                {
                    case "Crowbar":
                        itemsHUD[0].SetActive(true);
                        if (Inventory.Instance.itemNum != 0)
                        {
                            itemsHUD[Inventory.Instance.itemNum-1].SetActive(false);
                            Inventory.Instance.DropItem(Inventory.Instance.itemNum-1);
                        }
                        Inventory.Instance.itemNum = 1;
                        Destroy(hit.collider.gameObject);
                        break;
                    case "Hammer":
                        itemsHUD[1].SetActive(true);
                        if (Inventory.Instance.itemNum != 0)
                        {
                            itemsHUD[Inventory.Instance.itemNum - 1].SetActive(false);
                            Inventory.Instance.DropItem(Inventory.Instance.itemNum - 1);
                        }
                        Inventory.Instance.itemNum = 2;
                        Destroy(hit.collider.gameObject);
                        break;
                    case "Screwdriver":
                        itemsHUD[2].SetActive(true);
                        if (Inventory.Instance.itemNum != 0)
                        {
                            itemsHUD[Inventory.Instance.itemNum - 1].SetActive(false);
                            Inventory.Instance.DropItem(Inventory.Instance.itemNum - 1);
                        }
                        Inventory.Instance.itemNum = 3;
                        Destroy(hit.collider.gameObject);
                        break;
                    case "Handle":
                        itemsHUD[3].SetActive(true);
                        if (Inventory.Instance.itemNum != 0)
                        {
                            itemsHUD[Inventory.Instance.itemNum - 1].SetActive(false);
                            Inventory.Instance.DropItem(Inventory.Instance.itemNum - 1);
                        }
                        Inventory.Instance.itemNum = 4;
                        Destroy(hit.collider.gameObject);
                        break;
                    case "UnderGroundKey":
                        itemsHUD[4].SetActive(true);
                        if (Inventory.Instance.itemNum != 0)
                        {
                            itemsHUD[Inventory.Instance.itemNum - 1].SetActive(false);
                            Inventory.Instance.DropItem(Inventory.Instance.itemNum - 1);
                        }
                        Inventory.Instance.itemNum = 5;
                        Destroy(hit.collider.gameObject);
                        break;
                    case "SecretKey":
                        itemsHUD[5].SetActive(true);
                        if (Inventory.Instance.itemNum != 0)
                        {
                            itemsHUD[Inventory.Instance.itemNum - 1].SetActive(false);
                            Inventory.Instance.DropItem(Inventory.Instance.itemNum - 1);
                        }
                        Inventory.Instance.itemNum = 6;
                        Destroy(hit.collider.gameObject);
                        break;
                    case "MasterKey":
                        itemsHUD[6].SetActive(true);
                        if (Inventory.Instance.itemNum != 0)
                        {
                            itemsHUD[Inventory.Instance.itemNum - 1].SetActive(false);
                            Inventory.Instance.DropItem(Inventory.Instance.itemNum - 1);
                        }
                        Inventory.Instance.itemNum = 7;
                        Destroy(hit.collider.gameObject);
                        break;
                }

                UsedInteract();
            }
        }
        else if (Physics.Raycast(ray, out hit, 2f, 1 << 9)) //layermask 가 9일때 (note 일때)
        {
            InteractText.text = "노트 보기";
            InteractBtn.SetActive(true);

            if (isInteractPass || Input.GetKeyDown(KeyCode.E))
            {
                if (moveController.enabled)  //메모가 안켰다면
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
        else if (Physics.Raycast(ray, out hit, 2f, 1 << 10)) //layermask 가 10일때 (UseItemObj일때)
        {
            switch (hit.collider.gameObject.tag)
            {
                case "ironDoor":
                    if (Inventory.Instance.itemNum != 1) //가지고 있는 아이템이 쇠지렛대가 아니면
                    {
                        InteractText.text = "잠김";
                        return;
                    }

                    InteractBtn.SetActive(true);
                    InteractText.text = "아이템 사용";
                    if (isInteractPass || Input.GetKeyDown(KeyCode.E))
                    {
                        itemsHUD[Inventory.Instance.itemNum - 1].SetActive(false);
                        hit.collider.GetComponent<UseItem>().PlayAnim();
                        hit.collider.enabled = false;
                        Inventory.Instance.itemNum = 0;
                        UsedInteract();
                    }
                    break;
                case "Screw":
                    if (Inventory.Instance.itemNum != 3)
                    {
                        InteractText.text = "도구 필요";
                        return;
                    }

                    InteractBtn.SetActive(true);
                    InteractText.text = "아이템 사용";


                    if (isInteractPass || Input.GetKeyDown(KeyCode.E))
                    {
                        hit.collider.GetComponent<UseItem>().PlayAnim();
                        hit.collider.enabled = false;
                        Destroy(hit.collider.gameObject, 1f);
                        screwDurability--;
                        if (screwDurability == 0)
                        {
                            itemsHUD[Inventory.Instance.itemNum - 1].SetActive(false);
                            Inventory.Instance.itemNum = 0;
                            FrameRigid.useGravity = true;
                            FrameRigid.isKinematic = false;
                        }
                        UsedInteract();
                    }
                    break;
                case "Wood":
                    if (Durabilitywood == 0)
                    {
                        return;
                    }
                    if (Inventory.Instance.itemNum != 2)
                    {
                        InteractText.text = "도구 필요";
                        return;
                    }

                    InteractBtn.SetActive(true);
                    InteractText.text = "아이템 사용";

                    if (isInteractPass || Input.GetKeyDown(KeyCode.E))
                    {
                        hit.collider.GetComponent<Wood>().UseHammer();
                        Durabilitywood--;
                        if (GameManager.Instance.isFalseWood != 4 && Durabilitywood == 0)
                        {
                            Durabilitywood = 1;
                            return;
                        }
                        if (Durabilitywood == 0)
                        {
                            itemsHUD[Inventory.Instance.itemNum - 1].SetActive(false);
                            Inventory.Instance.itemNum = 0;

                            GameManager.Instance.isClearWood = true;
                            GameManager.Instance.SecretOpenCheck();
                        }
                        UsedInteract();
                    }
                    break;
                case "HandlePoint":
                    if (isUseHandle) //이미 핸들 끼웠다면
                        return;
                    if (Inventory.Instance.itemNum != 4)
                    {
                        InteractText.text = "핸들 필요";
                        return;
                    }

                    InteractBtn.SetActive(true);
                    InteractText.text = "아이템 사용";

                    if (isInteractPass || Input.GetKeyDown(KeyCode.E))
                    {
                        hit.collider.GetComponent<UseItem>().PlayAnim();
                        itemsHUD[Inventory.Instance.itemNum - 1].SetActive(false);
                        Inventory.Instance.itemNum = 0;
                        isUseHandle = true;
                        GameManager.Instance.isClearHandle = true;
                        GameManager.Instance.isClearCheck();
                        UsedInteract();
                    }
                    break;
                case "UnderGroundLock":
                    if (isUseUnderGroundLock)
                        return;
                    if (Inventory.Instance.itemNum != 5)
                    {
                        InteractText.text = "지하열쇠 필요";
                        return;
                    }

                    InteractBtn.SetActive(true);
                    InteractText.text = "아이템 사용";

                    if (isInteractPass || Input.GetKeyDown(KeyCode.E))
                    {
                        hit.collider.GetComponent<UseItem>().PlayAnim();
                        itemsHUD[Inventory.Instance.itemNum - 1].SetActive(false);
                        Inventory.Instance.itemNum = 0;

                        GameManager.Instance.isClearUnderLock = true;
                        isUseUnderGroundLock = true;//열었으면 텍스트 안뜨게
                        Invoke("SetFalseUnderLock", 1f);
                        UsedInteract();
                    }
                    break;
                case "SecretLock":
                    if (isUseSecretLock)
                        return;
                    if (Inventory.Instance.itemNum != 6)
                    {
                        InteractText.text = "시크릿키 필요";
                        return;
                    }

                    InteractBtn.SetActive(true);
                    InteractText.text = "아이템 사용";

                    if (isInteractPass || Input.GetKeyDown(KeyCode.E))
                    {

                        hit.collider.GetComponent<UseItem>().PlayAnim();
                        itemsHUD[Inventory.Instance.itemNum - 1].SetActive(false);
                        Inventory.Instance.itemNum = 0;
                        isUseSecretLock = true; //열었으면 텍스트 안뜨게

                        GameManager.Instance.isClearSecretLock = true;
                        GameManager.Instance.SecretOpenCheck();
                        UsedInteract();
                    }
                    break;
                case "MasterLock":
                    if (isUseMasterLock)
                        return;
                    if (Inventory.Instance.itemNum != 7)
                    {
                        InteractText.text = "마스터키 필요";
                        return;
                    }

                    InteractBtn.SetActive(true);
                    InteractText.text = "아이템 사용";

                    if (isInteractPass || Input.GetKeyDown(KeyCode.E))
                    {
                        hit.collider.GetComponent<UseItem>().PlayAnim();
                        itemsHUD[Inventory.Instance.itemNum - 1].SetActive(false);
                        Inventory.Instance.itemNum = 0;
                        isUseMasterLock = true;//열었으면 텍스트 안뜨게
                        GameManager.Instance.isClearMasterLock = true;
                        GameManager.Instance.isClearCheck();
                        UsedInteract();
                    }
                    break;
            }
        }
        
        else
        {
            InteractBtn.SetActive(false);
            InteractText.text = "";
        }

        Debug.DrawRay(transform.position + new Vector3(0, 1.7f, 0), cam.forward * 7f, Color.blue, 0.3f);
    }

    public void SetFalseUnderLock()
    {
        UnderLock.SetActive(false);
        GameManager.Instance.UnderOpen();
    }

    

    public void isDead()
    {
        moveController.enabled= false;
        transform.position = standPosition.position;
        Vector3 dir = monsterTrans.position - transform.position;
        transform.rotation = Quaternion.LookRotation(dir);
        Invoke("DeadCanvas", 2f);
    }

    public void DeadCanvas()
    {
        deadCanvas.SetActive(true);
    }

    private void OnControllerColliderHit(ControllerColliderHit collision)
    {
        if (collision.gameObject.tag == "Bed")
        {
            HideBtn.SetActive(true);
            hidePos = collision.transform.position;
            tablePos = transform.position;
            isCanHide = true; 
            isBed = true;
        }
        if (collision.gameObject.tag == "HideObj")
        {
            HideBtn.SetActive(true);
            hidePos = collision.transform.position + new Vector3(0.0f, -0.3f, 0f);
            tablePos = transform.position;
            isCanHide = true;
            isBed = false;
        }
    }


    public void OnClickHideBtn()
    {
        if (!isCanHide)
            return;

        if (isHiding)
        {
            moveController.isMove = true;
            transform.position = tablePos;

            isCanHide = true;
            isHiding = false;
            return;
        }

        if (isBed)
        {
            hidePos += Vector3.down * 1.45f;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        
        moveController.isMove = false;
        transform.position = hidePos;

        isHiding = true;
    }

    public void ClickIntaract()
    {
        isInteractPass = true;
    }
}
