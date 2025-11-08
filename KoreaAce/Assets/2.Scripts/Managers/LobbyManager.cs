using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyManager : MonoBehaviour
{
    public Button StartBtn;

    private void Start()
    {
        StartBtn.onClick.AddListener(() =>
        {
            GameSceneManager.Instance.LoadScene("GameScene");
        });
    }
}
