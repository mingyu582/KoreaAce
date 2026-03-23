using StarterAssets;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject OpeningCanvasObj;

    public FirstPersonController moveController;

    public NavMeshObstacle doorObs;
    public bool isClearUnderLock = false;
    public bool isClearHandle = false;
    public bool isClearMasterLock = false;

    public bool isClearWood = false;
    public bool isClearSecretLock = false;
    public GameObject SecretLock;

    public GameObject underHandle;
    public GameObject secretHandle;

    public GameObject clearPanel;
    public Animator cleaerAnim;

    public Monster monsterScript;

    //wood
    public int isFalseWood = 0;

    public AudioSource asource;

    private void Awake()
    {
        Instance= this;
    }

    private void Start()
    {
        OpeningCanvasObj.SetActive(true);
        Invoke("SetFalseOpening", 3f);
        underHandle.SetActive(false);
        secretHandle.SetActive(false);
    }

    public void isClearCheck()
    {
        if (isClearHandle && isClearMasterLock)
        {
            Invoke("ClearInvoke", 1f);
        }
    }

    public void ClearInvoke()
    {
        clearPanel.SetActive(true);
        cleaerAnim.SetBool("isClear", true);
        moveController.enabled = false;
    }

    public void SecretOpenCheck()
    {
        if (isClearWood && isClearSecretLock)
        {
            secretHandle.SetActive(true);
            Invoke("SetFalseSecretLock", 1f);
        }
    }

    public void UnderOpen()
    {
        doorObs.enabled = false;    
        underHandle.SetActive(true);
    }

    public void SetFalseSecretLock()
    {
        SecretLock.SetActive(false);
    }

    public void SetFalseOpening()
    {
        OpeningCanvasObj.SetActive(false);
    }





    public void OffSound()
    {
        asource.enabled = false;
    }
}
