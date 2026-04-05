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
            DontDestroyOnLoad(this);
            BGMPlay(BGMList[0]);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void FootStepPlay(bool isStep)
    {
        if (isStep)
        {
            if (!SFXSource.isPlaying)
                SFXSource.Play();
        }
        else
        {
            if (SFXSource.isPlaying)
                SFXSource.Stop();
        }
    }

    public void BGMPlay(AudioClip clip)
    {
        BGMSource.clip = clip;
        BGMSource.loop = true;
        BGMSource.volume = .5f;
        BGMSource.Play();
    }

    public void SFXPlay(AudioClip clip)
    {
        GameObject go = new GameObject("SFX");
        go.AddComponent<AudioSource>();
        go.GetComponent<AudioSource>().clip = clip;
        go.GetComponent<AudioSource>().loop = false;
        go.GetComponent<AudioSource>().volume = 1f;
        BGMSource.PlayOneShot(clip);
        Destroy(go, clip.length);
    }
}
