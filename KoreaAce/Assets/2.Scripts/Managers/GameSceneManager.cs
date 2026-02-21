using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance;
    public Monster monsterScript;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject); // 이미 존재하면 새로 만든 건 삭제
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
