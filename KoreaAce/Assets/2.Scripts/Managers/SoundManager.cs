using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public AudioSource BGMSource;
    public AudioSource SFXSource;

    public AudioClip[] BGMList;
    public AudioClip[] SFXList;

    public static SoundManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        for (int i = 0; i < BGMList.Length; i++)
        {
            if (arg0.name == BGMList[i].name)
            {
                BGMPlay(BGMList[i]);
            }
        }
    }

    public void SFXPlay(int num)
    {
        GameObject go = new GameObject("SFX Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.clip = SFXList[num];
        audioSource.Play();

        Destroy(go, SFXList[num].length);
    }

    public void BGMPlay(AudioClip clip)
    {
        BGMSource.clip = clip;
        BGMSource.loop = true;
        BGMSource.volume = 0.1f;
        BGMSource.Play();
    }
}
