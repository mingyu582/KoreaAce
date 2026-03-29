using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ToggleMainMenu : MonoBehaviour
{
    public Toggle Chapter1, Chapter2;
    public Button PlayBtn;
    public Image PlayImage;
    public bool isCanPlay = false;
    public int chapterNum = 0;

    public GameObject LoadingPanel;
    public Image Chapter2Img;
    public GameObject LockImage;

    void Start()
    {
        Chapter1.onValueChanged.AddListener(OnChapter1);
        Chapter2.onValueChanged.AddListener(OnChapter2);
        PlayBtn.onClick.AddListener(OnPlay);
        LoadingPanel.SetActive(false);
        //
        if (LockManager.isLock) //¿·∞‹¿÷¿∏∏È
        {
            Color newCol = new Color(1, 1, 1, .5f);
            Chapter2Img.color = newCol;
            LockImage.SetActive(true);
            Chapter2.enabled = false;
        }
        else
        {
            Color newCol = new Color(1, 1, 1, 1f);
            Chapter2Img.color = newCol;
            LockImage.SetActive(false);
            Chapter2.enabled = true;
        }
    }

    void OnChapter1(bool isOn)
    {
        if (isOn)
        {
            Debug.Log("√©≈Õ1 º±≈√");
            isCanPlay = true;
            Color newCol = new Color(1, 1, 1, 1f);
            PlayImage.color = newCol;
            chapterNum = 1;
        }
        else
        {
            Debug.Log("√©≈Õ1 º±≈√X");
            isCanPlay = false;
            Color newCol = new Color(1, 1, 1, 0.5f);
            PlayImage.color = newCol;
        }
    }

    void OnChapter2(bool isOn)
    {
        if (isOn)
        {
            Debug.Log("√©≈Õ2 º±≈√");
            isCanPlay = true;
            Color newCol = new Color(1, 1, 1, 1f);
            PlayImage.color = newCol;
            chapterNum = 2;
        }
        else
        {
            Debug.Log("√©≈Õ2 º±≈√X");
            isCanPlay = false;
            Color newCol = new Color(1, 1, 1, 0.5f);
            PlayImage.color = newCol;
        }
    }

    void OnPlay()
    {
        if (isCanPlay)
        {
            if (chapterNum == 1)
            {
                LoadingPanel.SetActive(true);
                Invoke("GoToChapter1", 2f);
            }
            else if (chapterNum == 2)
            {
                LoadingPanel.SetActive(true);
                Invoke("GoToChapter2", 2f);
            }
        }
        else
        {
            
        }
    }

    public void GoToChapter1()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void GoToChapter2()
    {
        SceneManager.LoadScene("StairScene");
    }
}
