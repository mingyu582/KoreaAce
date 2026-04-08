using UnityEngine;
using UnityEngine.SceneManagement;

public class StairManager : MonoBehaviour
{  
    public void GoToMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}
